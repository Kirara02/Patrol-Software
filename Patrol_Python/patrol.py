import os, sys
from dotenv import load_dotenv
import time
import asyncio
import ctypes
import platform
import shutil
import logging

from libecl import ECLStream

sys.path.append(os.path.join(os.path.dirname(__file__)))

# Setup logging
log_dir = r"C:\Uniguard\Patrol_Log"
os.makedirs(log_dir, exist_ok=True)  # Pastikan foldernya ada
log_file_path = os.path.join(log_dir, "activity.log")
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s - %(levelname)s - %(message)s",
    handlers=[
        logging.FileHandler(log_file_path, mode='w', encoding='utf-8'),  # Overwrite log file
        logging.StreamHandler(sys.stdout)
    ]
)

# Load .env
dotenv_path = os.path.join(os.path.dirname(__file__), ".env")
load_dotenv(dotenv_path=dotenv_path)

# Get paths from .env
LOG_DIR = os.getenv("LOG_PATH")
IMPORT_DIR = os.getenv("IMPORT_PATH")
ARCHIVE_DIR = os.getenv("ARCHIVE_PATH")
PASSWORD = os.getenv("PASSWORD", "6CF280F9-77AC-43B2-912D-73CED6F9439E")

# Log loaded env variables
logging.info("Loaded environment variables:")
logging.info(f"  LOG_PATH     = {LOG_DIR}")
logging.info(f"  IMPORT_PATH  = {IMPORT_DIR}")
logging.info(f"  ARCHIVE_PATH = {ARCHIVE_DIR}")

# Validate all required env variables
required_paths = {
    "LOG_PATH": LOG_DIR,
    "IMPORT_PATH": IMPORT_DIR,
    "ARCHIVE_PATH": ARCHIVE_DIR,
}

missing = [key for key, val in required_paths.items() if not val]
if missing:
    error_msg = f"❌ Environment variable(s) not set or empty: {', '.join(missing)}"
    logging.error(error_msg)
    raise Exception(error_msg)

# Path fix helper
def fix_path(path: str) -> str:
    if len(path) >= 2 and path[1] == ':' and path[2] not in ['/', '\\']:
        path = path[:2] + '/' + path[2:]
    return os.path.normpath(path)

def init_ecl():
    try:
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
        logging.info("ECL initialized successfully.")
    except Exception as e:
        logging.exception("Failed to initialize ECL")
        raise

async def encrypt_file(input_path):
    os.makedirs(IMPORT_DIR, exist_ok=True)
    os.makedirs(ARCHIVE_DIR, exist_ok=True)
    logging.info(f"Ensured folders exist: {IMPORT_DIR}, {ARCHIVE_DIR}")

    filename = os.path.basename(input_path)
    filename_wo_ext = os.path.splitext(filename)[0]
    output_path = os.path.join(IMPORT_DIR, filename_wo_ext + ".IMPORT")

    try:
        with open(input_path, "rb") as file_in, open(output_path, "wb") as file_out:
            ret = await ECLStream.write(
                password=PASSWORD,
                file_in=file_in,
                file_out=file_out,
            )
            logging.info(f"Encrypted file: {input_path} -> {output_path}")

        shutil.move(input_path, os.path.join(ARCHIVE_DIR, filename))
        logging.info(f"Archived file: {input_path} -> {ARCHIVE_DIR}")
        return ret
    except Exception as e:
        logging.exception(f"Failed to encrypt or move file: {input_path}")
        raise

async def monitor_loop():
    os.makedirs(LOG_DIR, exist_ok=True)
    logging.info(f"Monitoring folder: {LOG_DIR}")
    
    init_ecl()

    while True:
        try:
            files = [f for f in os.listdir(LOG_DIR) if f.endswith(".txt")]
            if len(files) == 1:
                input_path = os.path.join(LOG_DIR, files[0])
                logging.info(f"Detected new file: {input_path}")
                await encrypt_file(input_path)
            elif len(files) > 1:
                logging.warning("❗ Folder Logs berisi lebih dari 1 file. Tunggu hingga hanya 1.")
        except Exception as e:
            logging.exception("Unexpected error in monitoring loop.")
        await asyncio.sleep(3)

if __name__ == "__main__":
    logging.info("✅ Python script started successfully.")
    asyncio.run(monitor_loop())
