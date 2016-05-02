@echo off

echo To activate a specific window(e.g., Visual Studio): nircmd win activate stitle MultiTester

set config=%1
if %config%nul==nul set config=normal

if %config%==normal goto NORMAL
goto WIDESCREEN

:NORMAL
echo NORMAL
:: IBM
nircmd.exe win setsize stitle "IBM Rational ClearQuest" 45 190 1220 777
nircmd.exe win setsize stitle "Work Items - Rational" 70 170 1200 777
nircmd.exe win setsize title "ClearCase Remote Client" 93 155 1180 777

:: Outlook
nircmd.exe win setsize ititle "Microsoft Outlook" 30 215 1250 747

:: Visual Studio
nircmd.exe win setsize stitle "MultiTesterME" 115 55 1165 910

:: NP++
nircmd.exe win setsize etitle "Notepad++" 10 235 1250 730

goto:EOF

:WIDESCREEN
echo WIDESCREEN
REM :: IBM
nircmd.exe win setsize stitle "IBM Rational ClearQuest" 45 190 1220 647
nircmd.exe win setsize stitle "Work Items - Rational" 70 170 1200 647
nircmd.exe win setsize title "ClearCase Remote Client" 93 155 1180 647

:: Outlook
nircmd.exe win setsize ititle "Microsoft Outlook" 30 215 1250 617

:: Visual Studio
nircmd.exe win setsize stitle "MultiTesterME" 115 55 1165 780

:: NP++
nircmd.exe win setsize etitle "Notepad++" 10 235 1350 600



