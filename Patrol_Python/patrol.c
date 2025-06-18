#include <assert.h>
#include <stdbool.h>
#include <stdint.h>
#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <stdio.h>
#include <dlfcn.h>

#include "ecl/ripemd128.h"
#include "ecl/eclstream.h"

#ifndef USE_DLOPEN
#define USE_DLOPEN 1
#endif

int main(void) {
    void *libecl = dlopen("libecl.so", RTLD_NOW);
    if (!libecl) {
        fprintf(stderr, "libecl = %p\n", libecl);
        return 1;
    }
#if USE_DLOPEN
#define bind(x) typeof(x) *x = dlsym(libecl, #x); do {} while (0)
    bind(ripemd128_calc);
    bind(ECLStream_write_stackSize);
    bind(ECLStream_write_stackInit);
    bind(ECLStream_write);
#undef bind
#endif

    const char *repeated = "1,5,2147483647,06/06/2025,16:00:00,2147483647\r\n";
    const size_t repeatedSize = strlen(repeated);
    const size_t inputSize = repeatedSize * 1000000;
    char *input = malloc(inputSize);
    for (size_t i = 0; i < inputSize; i += repeatedSize) {
        memcpy(&input[i], repeated, repeatedSize);
    }
    const char *passwordText = "6CF280F9-77AC-43B2-912D-73CED6F9439E";
    uint8_t passwordHash[16];
    ripemd128_calc(passwordHash, (const uint8_t *) passwordText, strlen(passwordText));

    struct system_s system = (struct system_s){0};
    FILE *fileHandles[] = {NULL, NULL};
    const int fdIn = 0;
    fileHandles[fdIn] = fmemopen(input, inputSize, "rb");
    const int fdOut = 1;
    fileHandles[fdOut] = fopen("test-c.import", "wb");
    size_t totalSize;
    void *stack = ECLStream_write_stackInit(malloc(ECLStream_write_stackSize()),
                                            fdIn, fdOut, passwordHash, sizeof passwordHash, &totalSize);
    enum syscall_e call;
    while (call = ECLStream_write(stack, &system), call != SYSCALL_EXIT) {
        switch (call) {
            default: {
                assert(false && "unimplemented");
            }
            case SYSCALL_YIELD: {
                break;
            }
            case SYSCALL_MEM_ALLOC: {
                *system.mem.ret = malloc(system.mem.size);
                break;
            }
            case SYSCALL_MEM_REALLOC: {
                *system.mem.ret = realloc(system.mem.ptr, system.mem.size);
                break;
            }
            case SYSCALL_MEM_FREE: {
                free(system.mem.ptr);
                break;
            }
            case SYSCALL_IO_READ_REL: {
                const size_t n = fread(system.io.mBuf, 1, system.io.count, fileHandles[system.io.fd]);
                *system.io.ret = (ssize_t) n;
                break;
            }
            case SYSCALL_IO_WRITE_ABS: {
                fseek(fileHandles[system.io.fd], system.io.offset, SEEK_SET);
                const size_t n = fwrite(system.io.cBuf, 1, system.io.count, fileHandles[system.io.fd]);
                ssize_t *ret = system.io.ret;
                if (ret != NULL) *ret = (ssize_t) n;
                break;
            }
            case SYSCALL_IO_FLUSH: {
                fflush(fileHandles[system.io.fd]);
                fsync(fileno(fileHandles[system.io.fd]));
                break;
            }
        }
    }
    fprintf(stdout, "ret=%lu\n", totalSize);
    free(stack);
    fclose(fileHandles[fdOut]);
    fclose(fileHandles[fdIn]);
    free(input);
    return 0;
}
