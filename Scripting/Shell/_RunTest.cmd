@echo off

:: Variables that are assumed to be defined:
:: resultsDir

set assembly=%1
set fixture=%2
set switch=%3
set testToRun=%4
set showArgsOnly=%5
set nunit-exe="C:\Program Files\NUnit 2.5.6\bin\net-2.0\nunit-console.exe"

if NOT %showArgsOnly%nul==nul goto SHOWARGS
if %assembly%nul==nul goto ERROR
if %fixture%nul==nul goto ERROR
if %switch%nul==nul goto ERROR
if %testToRun%nul==nul goto ERROR

if NOT EXIST %assembly%.dll (
echo Error: %assembly%.dll not found
goto ERROR
)

if %switch%==-c goto RUNCATEGORY
if %switch%==-r goto RUNFUNCTION

goto ERROR

:SHOWARGS
echo assembly: %assembly%
echo fixture: %fixture%
echo switch: %switch%
echo testToRun: %testToRun%

goto END

:RUNCATEGORY
echo ***** Running category: %testToRun% in %assembly%.dll, fixture: %fixture% ***** >> %resultsDir%\nunit-log.txt

%nunit-exe% %assembly%.dll /fixture:%fixture% /include:%testToRun% >> %resultsDir%\nunit-log.txt

IF EXIST Results\%assembly%.Ver.docx MOVE /Y Results\%assembly%.Ver.docx %resultsDir%\%assembly%[%testToRun%].Ver.docx

echo ***** DONE ***** >> %resultsDir%\nunit-log.txt

goto END

:RUNFUNCTION
echo nunit-exe %assembly%.dll /run: %fixture%.%testToRun%
REM %nunit-exe% RepetitiveAndScanRateHysteresisImplant.dll /run:Msei.Verification.ImplantLevel.RepetitiveAndScanRateHysteresisPrimusCRTImplant.InteractionsProgramming.InteractionsProgramming
REM SHOULD BE THE SAME AS

goto END

:ERROR
echo "usage: RunTest assembly (without the .dll) <fixture> [-c category label or -r function to run]"

:END