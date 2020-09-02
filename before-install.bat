mkdir C:\inetpub\BK > NUL 2>&1
del /q C:\inetpub\BK\*
powershell -Command "Compress-Archive -Path C:\inetpub\wwwroot -DestinationPath C:\inetpub\bk\%date:~0,4%%date:~5,2%%date:~8,2%%time:~0,2%%time:~3,2%.zip"
cd C:\inetpub\wwwroot
del /q /s *.*
for /F %%a in ('dir /ad /b /w *') do rmdir /S /q %%a
c:\Windows\Sysnative\WindowsPowerShell\v1.0\powershell.exe -Command Import-Module -Name ServerManager
c:\Windows\Sysnative\WindowsPowerShell\v1.0\powershell.exe -Command Install-WindowsFeature Web-Server