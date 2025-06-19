import os, sys
from dotenv import load_dotenv

sys.path.append(os.path.join(os.path.dirname(__file__)))

import time
import asyncio
import ctypes
import platform
import shutil

from libecl import ECLStream

# Load .env
load_dotenv(dotenv_path=os.path.join(os.path.dirname(__file__), ".env"))

# Get paths from .env
LOG_DIR = os.getenv("LOG_PATH")
IMPORT_DIR = os.getenv("IMPORT_PATH")
ARCHIVE_DIR = os.getenv("ARCHIVE_PATH")
PASSWORD = os.getenv("PASSWORD", "6CF280F9-77AC-43B2-912D-73CED6F9439E")

# Validate all required env variables
required_paths = {
    "LOG_PATH": LOG_DIR,
    "IMPORT_PATH": IMPORT_DIR,
    "ARCHIVE_PATH": ARCHIVE_DIR,
}

missing = [key for key, val in required_paths.items() if not val]
if missing:
    raise Exception(f"❌ Environment variable(s) not set or empty: {', '.join(missing)}")

# Path fix helper
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

async def encrypt_file(input_path):
    os.makedirs(IMPORT_DIR, exist_ok=True)
    os.makedirs(ARCHIVE_DIR, exist_ok=True)

    filename = os.path.basename(input_path)
    filename_wo_ext = os.path.splitext(filename)[0]
    output_path = os.path.join(IMPORT_DIR, filename_wo_ext + ".IMPORT")

    with open(input_path, "rb") as file_in, open(output_path, "wb") as file_out:
        ret = await ECLStream.write(
            password=PASSWORD,
            file_in=file_in,
            file_out=file_out,
        )
        print(f"Encrypted {filename} -> {output_path}")

    shutil.move(input_path, os.path.join(ARCHIVE_DIR, filename))
    print(f"Archived: {filename}")
    return ret

async def monitor_loop():
    os.makedirs(LOG_DIR, exist_ok=True)

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
