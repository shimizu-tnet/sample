cd C:\inetpub\wwwroot\
del /q /s *.*
for /F %%a in ('dir /ad /b /w *') do rmdir /S /q %%a
xcopy c:\inetpub\deploytmp\JuniorTennis.Mvc\bin\Debug\netcoreapp3.1 C:\inetpub\wwwroot /E /I
rd c:\inetpub\deploytmp /S /Q
c:\Windows\Sysnative\WindowsPowerShell\v1.0\powershell.exe -Command Import-Module -Name ServerManager
c:\Windows\Sysnative\WindowsPowerShell\v1.0\powershell.exe -Command Install-WindowsFeature Web-Server
