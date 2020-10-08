iisreset /stop
cd C:\inetpub\wwwroot\
del /q /s *.*
for /F %%a in ('dir /ad /b /w *') do rmdir /S /q %%a
xcopy c:\inetpub\deploytmp C:\inetpub\wwwroot /E /I
rd c:\inetpub\deploytmp /S /Q
iisreset /start
