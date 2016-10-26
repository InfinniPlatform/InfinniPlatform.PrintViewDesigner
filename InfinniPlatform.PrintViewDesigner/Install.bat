powershell -NoProfile -ExecutionPolicy Bypass -Command "& { Invoke-WebRequest -Uri 'https://raw.githubusercontent.com/InfinniPlatform/InfinniPlatform.PrintViewDesigner/master/InfinniPlatform.PrintViewDesigner/Install.ps1' -OutFile 'Install.ps1' }"
powershell -NoProfile -ExecutionPolicy Bypass -Command ".\Install.ps1" %1
if exist "Install.ps1" del /F "Install.ps1"