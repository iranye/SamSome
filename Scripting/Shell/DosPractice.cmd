@echo off
set STARTTIME=%TIME%
echo %STARTTIME%

pause
REM call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x86

REM mstest /testcontainer:Test\UnitTest\bin\Debug\Liport.UnitTest.dll /category:"!LongRunning" /testsettings:LocalTestRun.testrunconfig /detail:duration
REM mstest /testcontainer:Test\UnitTest\bin\Debug\Liport.UnitTest.dll /category:"!LongRunning" /testsettings:LocalTestRun_Coverage.testrunconfig /detail:duration

set ENDTIME=%TIME%

REM centiseconds - 1/100 seconds
REM set /A STARTTIME=(1%STARTTIME:~0,2%-100)*360000 + (1%STARTTIME:~3,2%-100)*6000 + (1%STARTTIME:~6,2%-100)*100 + (1%STARTTIME:~9,2%-100)
REM set /A ENDTIME=(1%ENDTIME:~0,2%-100)*360000 + (1%ENDTIME:~3,2%-100)*6000 + (1%ENDTIME:~6,2%-100)*100 + (1%ENDTIME:~9,2%-100)

REM seconds
set /A STARTTIME=(1%STARTTIME:~0,2%-100)*3600 + (1%STARTTIME:~3,2%-100)*60 + (1%STARTTIME:~6,2%-100)
set /A ENDTIME=(1%ENDTIME:~0,2%-100)*3600 + (1%ENDTIME:~3,2%-100)*60 + (1%ENDTIME:~6,2%-100)

set /a RUNTIME=%ENDTIME%-%STARTTIME%

echo Script took %RUNTIME% seconds to complete

pause
