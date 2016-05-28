@echo off
for /f "TOKENS=1 DELIMS=:" %%A IN ('TIME/T') DO SET HH=%%A
REM echo %HH%

set logfile=NotYetPassed_%date:~4,2%%date:~7,2%%date:~-4%_%HH%%time:~3,2%%time:~6,2%.log
echo %logfile%
pause

set src="\\soong\PrimusCRT\Implant Verification\Test Results"
set dest=C:\TestResults\TestResultsNotYetPassedTests
set p=/e /s /R:2 /W:5 /np /tee /xd _tobedeleted

robocopy %src% %dest% %p% /log+:C:\TestResults\%logfile%

echo result saved to logfile: C:\TestResults\%logfile%

pause
set src="\\soong\PrimusCRT\Verification\Implant Verification\Test Results\Increment 3.1"
set dest=C:\TestResults\TestResultsPassedTests

set logfile=PassedTests_%date:~4,2%%date:~7,2%%date:~-4%_%HH%%time:~3,2%%time:~6,2%.log
robocopy %src% %dest% %p% /log+:C:\TestResults\%logfile%

echo result saved to logfile: C:\TestResults\%logfile%