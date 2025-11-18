@echo off
echo ========================================
echo  Registering HTTP URL for WCF Service
echo ========================================
echo.
echo This script must run as Administrator!
echo.

REM Register the URL for the Social Insurance Service
netsh http add urlacl url=http://+:8080/SocialInsuranceService/ user=Everyone

if %ERRORLEVEL% EQU 0 (
    echo.
    echo [SUCCESS] URL registered successfully!
    echo You can now run the HostApplication without administrator privileges.
) else (
    echo.
    echo [ERROR] Failed to register URL. Error code: %ERRORLEVEL%
    echo Make sure you ran this script as Administrator.
)

echo.
pause
