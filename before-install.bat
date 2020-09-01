REM Install Internet Information Server (IIS).
cd C:\inetpub\wwwroot
del /q /s *.*
for /F %%a in ('dir /ad /b /w *') do rmdir /S /q %%a
c:\Windows\Sysnative\WindowsPowerShell\v1.0\powershell.exe -Command Import-Module -Name ServerManager
c:\Windows\Sysnative\WindowsPowerShell\v1.0\powershell.exe -Command Install-WindowsFeature Web-Server