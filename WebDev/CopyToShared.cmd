@echo off
set target=C:\Shared\Regexer_Dev
echo Copying files to: %target%
pause
robocopy .\Regexer %target% /e /s /xd bin /xd obj /xd  /xf *.user
REM copy /y C:\Shared\Regexer_Dev\objects.js Regexer
REM copy /y C:\Shared\Regexer_Dev\Regexer.html Regexer

pause
