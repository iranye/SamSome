@echo off

:: *** Core.Data Testing ***
:: CD C:\src\git\library-coredata\Tests\NVP.Core.Data.Sql.Tests.ZpDatabases\bin\Release
:: NVP.ICCP.IntegrationTests.dll
vstest.console.exe NVP.ICCP.IntegrationTests.dll /Logger:trx /ResultsDirectory:C:\ProgramData\TestResults\NVP.ICCP.IntegrationTests\VSTest
vstest.console.exe NVP.Core.Data.Sql.Tests.ZpDatabases.dll /Logger:trx /ResultsDirectory:C:\ProgramData\TestResults\NVP.Core.Data.TestResults\VSTest /TestCaseFilter:TestCategory=ZpCustomers

:: *** ICCP Testing ***
:: CD C:\source\Nvoicepay\ICCP\Libraries\NVP.ICCP\NVP.ICCP.IntegrationTests\bin\Debug
:: NVP.ICCP.IntegrationTests.dll

::  /TestCaseFilter:TestCategory=AuthsAndClearances
::  /TestCaseFilter:TestCategory=VcnRequest
::  /TestCaseFilter:TestCategory=BasicInfo
::  /TestCaseFilter:TestCategory=VcnDetail
::  /TestCaseFilter:TestCategory=ModifyVcn
::  /TestCaseFilter:TestCategory=CancelVcn
::  /TestCaseFilter:TestCategory=


:: vstest.console.exe %1 /TestCaseFilter:TestCategory=Nightly /Logger:trx
:: vstest.console.exe NVP.ICCP.IntegrationTests.dll /Logger:trx

SET WAITCMD=C:\Windows\System32\timeout.exe
SET ITERCOUNT=8
:: SET WAITSECONDS_DEFAULT=5
SET WAITSECONDS_24_HOURS=86400
SET WAITSECONDS_12_HOURS=43200

:REPEAT
IF %ITERCOUNT%==0 goto:eof

SET /A ITERCOUNT=ITERCOUNT-1
echo %ITERCOUNT% 

vstest.console.exe NVP.ICCP.IntegrationTests.dll /Logger:trx /ResultsDirectory:C:\ProgramData\TestResults\NVP.ICCP.IntegrationTests\VSTest

%WAITCMD% %WAITSECONDS_12_HOURS%
GOTO REPEAT

goto:eof

:: CD C:\source\Nvoicepay\VSTS-17623-Part2\Services\NVP.EpiqPrintCheckRequestFileProcessor\NVP.EpiqPrintCheckRequestFileProcessor.Test\bin\Debug
NVP.EpiqPrintCheckRequestFileProcessor.dll
