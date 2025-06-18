import os, sys
sys.path.append(os.path.join(os.path.dirname(__file__)))

import time
import asyncio
import ctypes
import platform
import shutil

from libecl import ECLStream

LOG_DIR = "C:/Patrol_Log/Logs/"
IMPORT_DIR = "C:/Patrol_Log/Import/"
ARCHIVE_DIR = "C:/Patrol_Log/Archives/"
PASSWORD = "6CF280F9-77AC-43B2-912D-73CED6F9439E"

def fix_path(path: str) -> str:
    if len(path) >= 2 and path[1] == ':' and path[2] not in ['/', '\\']:
        path = path[:2] + '/' + path[2:]
    return os.path.normpath(path)

def init_ecl():
    libecl = {
        "Windows": lambda: ctypes.CDLL("./libecl.dll"),
        "Linux": lambda: ctypes.CDLL("libecl.so"),
    }[platform.system()]()
    libc = {
        "Windows": lambda: ctypes.CDLL("msvcrt"),
        "Linux": lambda: ctypes.CDLL("libc.so.6"),
    }[platform.system()]()
    ECLStream.init(
        libecl=libecl,
        libc=libc,
        memory_managed=True,
    )
    
async def  encrypt_file(input_path):
    os.makedirs(IMPORT_DIR, exist_ok=True)
    os.makedirs(ARCHIVE_DIR, exist_ok=True)

    filename = os.path.basename(input_path)
    filename_wo_ext = os.path.splitext(filename)[0]
    output_path = os.path.join(IMPORT_DIR, filename_wo_ext + ".import")

    with open(input_path, "rb") as file_in, open(output_path, "wb") as file_out:
        ret = await ECLStream.write(
            password=PASSWORD,
            file_in=file_in,
            file_out=file_out,
        )
        print(f"Encrypted {filename} -> {output_path}")
    
    # Pindahkan file txt ke archive
    shutil.move(input_path, os.path.join(ARCHIVE_DIR, filename))
    print(f"Archived: {filename}")
    return ret

async def monitor_loop():
    init_ecl()
    print("Monitoring folder:", LOG_DIR)
    while True:
        files = [f for f in os.listdir(LOG_DIR) if f.endswith(".txt")]
        if len(files) == 1:
            input_path = os.path.join(LOG_DIR, files[0])
            await encrypt_file(input_path)
        elif len(files) > 1:
            print("❗ Folder Logs berisi lebih dari 1 file. Tunggu hingga hanya 1.")
        await asyncio.sleep(3)
            

if __name__ == "__main__":
    asyncio.run(monitor_loop())
