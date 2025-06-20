@echo off
:: Cek apakah sudah dijalankan sebagai admin
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo Menjalankan ulang sebagai Administrator...
    powershell -Command "Start-Process '%~f0' -Verb RunAs"
    exit /b
)

echo -------------------------------
echo Registering WmWebUsb.dll ...
echo -------------------------------

set "dllPath=%~dp0WmWebUsb.dll"
echo DLL Path: %dllPath%

regsvr32 /s "%dllPath%"

if %ERRORLEVEL% EQU 0 (
    echo ✅ DLL registered successfully.
) else (
    echo ❌ Failed to register DLL.
)

pause
