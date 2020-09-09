mkdir C:\inetpub\deploytmp > NUL 2>&1
cd C:\inetpub\deploytmp\
del /q /s *.*
for /F %%a in ('dir /ad /b /w *') do rmdir /S /q %%a

