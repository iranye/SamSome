@echo off

set massagedTime=%TIME: =0%

REM for /f "TOKENS=1 DELIMS=:" %%A IN ('TIME/T') DO SET HH=%%A

set HH=%massagedTime:~0,2%
echo HH=%HH%

echo %date%
echo %time%

set timestamp=%date:~4,2%%date:~7,2%%date:~-4%_%HH%%time:~3,2%%time:~6,2%
echo %timestamp%
REM for /f "TOKENS=1 DELIMS=:" %%A IN ('TIME/T') DO SET HH=%%A
REM echo %HH%

REM set logfile=NotYetPassed_%date:~4,2%%date:~7,2%%date:~-4%_%HH%%time:~3,2%%time:~6,2%.log
REM echo %logfile%
REM pause

REM set src="\\soong\PrimusCRT\Implant Verification\Test Results"
REM set dest=C:\TestResults\TestResultsNotYetPassedTests
REM set p=/e /s /R:2 /W:5 /np /tee /xd _tobedeleted

REM robocopy %src% %dest% %p% /log+:C:\TestResults\%logfile%

REM echo result saved to logfile: C:\TestResults\%logfile%

REM pause
REM set src="\\soong\PrimusCRT\Verification\Implant Verification\Test Results\Increment 3.1"
REM set dest=C:\TestResults\TestResultsPassedTests

REM set logfile=PassedTests_%date:~4,2%%date:~7,2%%date:~-4%_%HH%%time:~3,2%%time:~6,2%.log
REM robocopy %src% %dest% %p% /log+:C:\TestResults\%logfile%

REM echo result saved to logfile: C:\TestResults\%logfile%