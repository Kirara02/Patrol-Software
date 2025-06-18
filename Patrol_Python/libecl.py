import asyncio
import ctypes
import io
import os
import typing


class SystemMem(ctypes.Structure):
    _fields_ = [
        ("ptr", ctypes.c_void_p),
        ("size", ctypes.c_size_t),
        ("ret", ctypes.POINTER(ctypes.c_void_p)),
    ]


class SystemIO(ctypes.Structure):
    _fields_ = [
        ("fd", ctypes.c_int),
        ("buf", ctypes.c_void_p),
        ("count", ctypes.c_size_t),
        ("offset", ctypes.c_size_t),
        ("ret", ctypes.POINTER(ctypes.c_ssize_t)),
    ]


class System(ctypes.Structure):
    _fields_ = [
        ("mem", SystemMem),
        ("io", SystemIO),
    ]


class ECLStream:
    _libecl: ctypes.CDLL = None
    memory_managed = False
    _libc: typing.Optional[ctypes.CDLL] = None

    @classmethod
    def init(cls, libecl: ctypes.CDLL, memory_managed: bool = True, libc: typing.Optional[ctypes.CDLL] = None) -> None:
        cls.memory_managed = memory_managed

        def bind(sym, args, res):
            sym.argtypes = args
            sym.restype = res

        bind(sym=libecl.ripemd128_calc, args=[ctypes.c_void_p, ctypes.c_void_p, ctypes.c_size_t], res=None)
        bind(sym=libecl.ECLStream_write_stackSize, args=[], res=ctypes.c_size_t)
        bind(
            sym=libecl.ECLStream_write_stackInit,
            args=[ctypes.c_void_p, ctypes.c_int, ctypes.c_int, ctypes.c_void_p, ctypes.c_size_t, ctypes.c_void_p],
            res=ctypes.c_void_p
        )
        bind(sym=libecl.ECLStream_write, args=[ctypes.c_void_p, ctypes.c_void_p], res=ctypes.c_int)
        cls._libecl = libecl

        if libc:
            bind(sym=libc.malloc, args=[ctypes.c_size_t], res=ctypes.c_void_p)
            bind(sym=libc.realloc, args=[ctypes.c_void_p, ctypes.c_size_t], res=ctypes.c_void_p)
            bind(sym=libc.free, args=[ctypes.c_void_p], res=None)
            cls._libc = libc

    @classmethod
    def _memcpy(cls, dst: typing.Any, src: typing.Union[bytes, typing.Any], size: int) -> None:
        ctypes.memmove(dst, src, size)

    @classmethod
    def _marshal_bytearray(cls, value: bytearray, offset: int = 0) -> typing.Any:
        size = len(value)
        array_type = ctypes.c_byte * (size - offset)
        return array_type.from_buffer(value, offset)

    @classmethod
    def _unmarshal_bytearray(cls, value: typing.Any, size: int) -> typing.Any:
        ret = ctypes.string_at(value, size)
        return ret

    class Allocation:
        def __init__(self, value: int, memory: bytearray, offset: int, size: int):
            self.value = value
            self.memory = memory
            self.offset = offset
            self.size = size

        def __repr__(self):
            return f"<Allocation value={hex(self.value)} size={self.size}>"

    _allocations = {}

    # alignof(max_align_t)
    _alignof_max_align_t = max(
        ctypes.alignment(ctypes.c_void_p),
        ctypes.alignment(ctypes.c_size_t),
        ctypes.alignment(ctypes.c_int64),
        ctypes.alignment(ctypes.c_longlong),
        ctypes.alignment(ctypes.c_longdouble),
    )

    @classmethod
    def _malloc(cls, size: int) -> typing.Any:
        if cls.memory_managed:
            alignment = cls._alignof_max_align_t
            ext_size = size + (alignment - 1)
            b = bytearray(ext_size)
            ext_mem = ctypes.addressof(cls._marshal_bytearray(b))
            offset = ((ext_mem + (alignment - 1)) & ~(alignment - 1)) - ext_mem
            mem = ctypes.addressof(cls._marshal_bytearray(b, offset))
            cls._allocations[mem] = cls.Allocation(mem, b, offset, size)
            return mem
        else:
            return cls._libc.malloc(size)

    @classmethod
    def _realloc(cls, mem: typing.Any, size: int) -> typing.Any:
        if cls.memory_managed:
            allocation = cls._allocations.get(mem)
            if allocation and allocation.size == size:
                return mem
            del cls._allocations[mem]
            replacement = cls._malloc(size)
            if allocation:
                cls._memcpy(replacement, allocation.value, min(allocation.size, size))
            return replacement
        else:
            return cls._libc.realloc(mem, size)

    @classmethod
    def _free(cls, mem: typing.Any) -> None:
        if cls.memory_managed:
            del cls._allocations[mem]
        else:
            cls._libc.free(mem)

    @classmethod
    def _ripemd128_calc(cls, buf: bytes) -> bytes:
        ret = bytearray(16)
        cls._libecl.ripemd128_calc(cls._marshal_bytearray(ret), cls._marshal_bytearray(bytearray(buf)), len(buf))
        return bytes(ret)

    @classmethod
    def _eclstream_write_stack_size(cls) -> int:
        return cls._libecl.ECLStream_write_stackSize()

    @classmethod
    def _eclstream_write_stack_new(cls, fd_in: int, fd_out: int, key: bytes,
                                   out: typing.Any) -> typing.Any:
        size = cls._eclstream_write_stack_size()
        mem = cls._malloc(size)
        mem = cls._libecl.ECLStream_write_stackInit(mem, fd_in, fd_out, key, len(key), out)
        return mem

    @classmethod
    def _eclstream_write(cls, stack: typing.Any, system: System) -> int:
        return cls._libecl.ECLStream_write(stack, ctypes.byref(system))

    @classmethod
    async def write(cls, password: str, file_in: typing.BinaryIO, file_out: typing.BinaryIO) -> int:
        key = cls._ripemd128_calc(password.encode("utf-8"))
        fd_in = 0
        fd_out = 1
        files = {fd_in: file_in, fd_out: file_out}
        system = System()

        total_len = ctypes.c_ssize_t()
        stack = cls._eclstream_write_stack_new(fd_in, fd_out, key, ctypes.byref(total_len))

        def iop_read_rel(f: typing.BinaryIO, n: int) -> bytes:
            return f.read(n)

        def iop_write_abs(f: typing.BinaryIO, o: int, b: bytes) -> int:
            f.seek(o, io.SEEK_SET)
            return f.write(b)

        def iop_flush(f: typing.BinaryIO) -> None:
            f.flush()
            os.fsync(f.fileno())

        while True:
            call = cls._eclstream_write(stack, system)
            if call == 1:  # exit
                break
            elif call == 2:  # yield
                await asyncio.sleep(0)
            elif call == 3:  # mem_alloc
                mem = cls._malloc(system.mem.size)
                system.mem.ret.contents.value = mem
            elif call == 4:  # mem_realloc
                mem = cls._realloc(system.mem.ptr, system.mem.size)
                system.mem.ret.contents.value = mem
            elif call == 5:  # mem_free
                cls._free(system.mem.ptr)
            elif call == 6:  # io_read_rel
                b = await asyncio.to_thread(iop_read_rel, files[system.io.fd], system.io.count)
                n = len(b)
                cls._memcpy(system.io.buf, b, n)
                system.io.ret.contents.value = n
            elif call == 9:  # io_write_abs
                n = await asyncio.to_thread(iop_write_abs, files[system.io.fd], system.io.offset,
                                            cls._unmarshal_bytearray(system.io.buf, system.io.count))
                if system.io.ret:
                    system.io.ret.contents.value = n
            elif call == 10:  # io_flush
                await asyncio.to_thread(iop_flush, files[system.io.fd])
            else:
                assert False, "unimplemented"
        cls._free(stack)
        return total_len.value
