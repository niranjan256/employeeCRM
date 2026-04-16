@echo off
setlocal EnableDelayedExpansion
color 0A
title Employee CRM - Setup Script

echo.
echo  ============================================================
echo   Employee CRM - Automated Setup Script
echo   Pavan Kalyan Project
echo  ============================================================
echo.

:: ─────────────────────────────────────────────────────────────
:: STEP 1 — Check .NET 8 SDK
:: ─────────────────────────────────────────────────────────────
echo [1/5] Checking .NET 8 SDK...
dotnet --list-sdks 2>nul | findstr /R "^8\." >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo  [ERROR] .NET 8 SDK is NOT installed.
    echo.
    echo  Please download and install it from:
    echo  https://dotnet.microsoft.com/en-us/download/dotnet/8.0
    echo.
    echo  After installing, re-run this script.
    echo.
    pause
    exit /b 1
)
for /f "tokens=1" %%v in ('dotnet --list-sdks ^| findstr /R "^8\."') do (
    echo  [OK] .NET SDK %%v found.
    goto :dotnet_ok
)
:dotnet_ok

:: ─────────────────────────────────────────────────────────────
:: STEP 2 — Check SQL Server LocalDB
:: ─────────────────────────────────────────────────────────────
echo.
echo [2/5] Checking SQL Server LocalDB...
sqllocaldb info >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo  [WARNING] SQL Server LocalDB was not found in PATH.
    echo.
    echo  Attempting to install via winget...
    winget --version >nul 2>&1
    if %ERRORLEVEL% NEQ 0 (
        echo.
        echo  [ERROR] winget is also not available.
        echo.
        echo  Please install SQL Server LocalDB manually:
        echo.
        echo  Option A (Recommended - Free):
        echo    Download SQL Server Express with LocalDB from:
        echo    https://www.microsoft.com/en-us/sql-server/sql-server-downloads
        echo    (Choose "Express" edition, then select LocalDB in the installer)
        echo.
        echo  Option B:
        echo    Install Visual Studio 2022 Community (includes LocalDB by default)
        echo.
        echo  After installing, restart your PC and re-run this script.
        echo.
        pause
        exit /b 1
    )

    echo  Installing SQL Server 2022 LocalDB via winget...
    winget install --id Microsoft.SQLServer.2022.Express --silent --accept-package-agreements --accept-source-agreements
    if %ERRORLEVEL% NEQ 0 (
        echo.
        echo  [ERROR] Automatic install failed.
        echo  Please install LocalDB manually from:
        echo  https://www.microsoft.com/en-us/sql-server/sql-server-downloads
        echo.
        pause
        exit /b 1
    )
    echo  [OK] SQL Server LocalDB installed. You may need to restart your PC.
    echo.
) else (
    for /f "tokens=*" %%i in ('sqllocaldb info 2^>^&1 ^| findstr /R "."') do (
        echo  [OK] LocalDB found. Instances: %%i
        goto :localdb_ok
    )
    echo  [OK] SQL Server LocalDB is available.
    :localdb_ok
)

:: Ensure the default MSSQLLocalDB instance is running
echo  Starting MSSQLLocalDB instance...
sqllocaldb start MSSQLLocalDB >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo  [INFO] Could not start MSSQLLocalDB (it may already be running or use a different instance name).
) else (
    echo  [OK] MSSQLLocalDB is running.
)

:: ─────────────────────────────────────────────────────────────
:: STEP 3 — Check EF Core CLI Tools
:: ─────────────────────────────────────────────────────────────
echo.
echo [3/5] Checking EF Core CLI tools...
dotnet ef --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo  [INFO] dotnet-ef tool not found. Installing globally...
    dotnet tool install --global dotnet-ef --version 8.0.*
    if %ERRORLEVEL% NEQ 0 (
        echo  [WARNING] Failed to install dotnet-ef globally.
        echo  The project auto-migrates on first run, so this may not be needed.
        echo  If you encounter database errors, run:
        echo    dotnet tool install --global dotnet-ef --version 8.0.*
    ) else (
        echo  [OK] dotnet-ef installed.
        :: Refresh PATH so dotnet-ef is found in this session
        set "PATH=%PATH%;%USERPROFILE%\.dotnet\tools"
    )
) else (
    for /f "tokens=*" %%v in ('dotnet ef --version 2^>^&1') do (
        echo  [OK] dotnet-ef %%v found.
        goto :ef_ok
    )
    :ef_ok
)

:: ─────────────────────────────────────────────────────────────
:: STEP 4 — Restore NuGet Packages
:: ─────────────────────────────────────────────────────────────
echo.
echo [4/5] Restoring NuGet packages...
dotnet restore "%~dp0EmployeeCRM.sln"
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo  [ERROR] NuGet restore failed.
    echo  Make sure you have an internet connection and try again.
    echo.
    pause
    exit /b 1
)
echo  [OK] All packages restored.

:: ─────────────────────────────────────────────────────────────
:: STEP 5 — Build the Solution
:: ─────────────────────────────────────────────────────────────
echo.
echo [5/5] Building the solution...
dotnet build "%~dp0EmployeeCRM.sln" --no-restore --configuration Debug
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo  [ERROR] Build failed. See errors above.
    echo  Common fixes:
    echo    - Make sure .NET 8 SDK is installed (not just runtime)
    echo    - Delete bin/ and obj/ folders and retry
    pause
    exit /b 1
)
echo  [OK] Build succeeded.

:: ─────────────────────────────────────────────────────────────
:: LAUNCH — Start both projects
:: ─────────────────────────────────────────────────────────────
echo.
echo  ============================================================
echo   Setup complete! Starting both projects...
echo  ============================================================
echo.
echo  The database will be created and seeded automatically on
echo  first startup (this may take a few seconds).
echo.
echo  Default login credentials:
echo    Admin    - Username: admin        Password: Admin@123
echo    Manager  - Username: ravi.kumar   Password: Manager@123
echo    Employee - Username: pavan.kalyan Password: Employee@123
echo.
echo  API (Swagger UI):  https://localhost:7001/swagger
echo  Web App (MVC):     https://localhost:7136
echo.
echo  Starting API in a new window...
start "Employee CRM - API" cmd /k "cd /d "%~dp0src\EmployeeCRM.API" && dotnet run --launch-profile https"

echo  Waiting 8 seconds for API to initialize...
timeout /t 8 /nobreak >nul

echo  Starting MVC in a new window...
start "Employee CRM - MVC" cmd /k "cd /d "%~dp0src\EmployeeCRM.MVC" && dotnet run --launch-profile https"

echo  Waiting 5 seconds for MVC to start...
timeout /t 5 /nobreak >nul

echo.
echo  Opening browser...
start https://localhost:7136

echo.
echo  Both projects are running in separate windows.
echo  Close those windows to stop the servers.
echo.
pause
