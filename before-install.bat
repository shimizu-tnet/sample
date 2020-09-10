cd C:\inetpub\wwwroot\
del /q /s *.*
for /F %%a in ('dir /ad /b /w *') do rmdir /S /q %%a
