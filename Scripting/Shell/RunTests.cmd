:: Set up assembly name and fixture, then for each category or run function needed to run, run nUnit with the appropriate parameters
:: The script is intended to be saved to a batch file and ran in the folder: C:\bio\UTE\Implant\Test
@echo off
SET assembly=RepetitiveAndScanRateHysteresisImplant
SET fixture=Msei.Verification.ImplantLevel.RepetitiveAndScanRateHysteresisPrimusCRTImplant

SET resultsDir=_Results_%assembly%
IF NOT EXIST %resultsDir% MKDIR %resultsDir%
IF EXIST _nunit-log.txt del _nunit-log.txt
SET nunit-exe="C:\Program Files\NUnit 2.5.6\bin\net-2.0\nunit-console.exe"

SET category=InteractionsProgramming
%nunit-exe% %assembly%.dll /include=%category% /fixture=%fixture% >> _nunit-log.txt
IF EXIST Results\%assembly%.Ver.docx MOVE /Y Results\%assembly%.Ver.docx %resultsDir%\%assembly%[%category%].Ver.docx

SET function=RepetitiveRateHysteresisVsInPSync
%nunit-exe% %assembly%.dll /run=%fixture%.%function% >> _nunit-log.txt
IF EXIST Results\%assembly%.Ver.docx MOVE /Y Results\%assembly%.Ver.docx %resultsDir%\%assembly%(%function%).Ver.docx

SET category=RepetitiveRateHysteresisEventsVsInPSyncMode
%nunit-exe% %assembly%.dll /include=%category% /fixture=%fixture% >> _nunit-log.txt
IF EXIST Results\%assembly%.Ver.docx MOVE /Y Results\%assembly%.Ver.docx %resultsDir%\%assembly%[%category%].Ver.docx


IF EXIST _nunit-log.txt MOVE /Y _nunit-log.txt %resultsDir%\nunit-log.txt
