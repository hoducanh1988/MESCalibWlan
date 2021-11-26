@echo off

SET SUBDIR=%~dp0
ECHO %SUBDIR%

if exist "%SUBDIR%"QMSL_MSVC10R.dll DEL "%SUBDIR%"QMSL_MSVC10R.dll

:: Find out Windows version
for /f "delims=" %%a in ('systeminfo ^| findstr /B /C:"OS Name"') do @set OS_NAME=%%a 
echo %OS_NAME%

echo %OS_NAME% | findstr /C:"Windows XP" 1>nul


:: Check if the OS is Windows 7 if so run the sequence in else part

if errorlevel 1 (

echo Win 7 detected
mklink "%SUBDIR%"QMSL_MSVC10R.dll "%SUBDIR%"QCAMSL_MSVC10R.dll

) ELSE (

echo XP detected
fsutil hardlink create "%SUBDIR%"QMSL_MSVC10R.dll "%SUBDIR%"QCAMSL_MSVC10R.dll

)

:exit
