
@echo off

echo To activate a specific window(e.g., Visual Studio): nircmd win activate stitle MultiTester

echo nircmd.exe win setsize etitle "cmd.exe" 940 690 940 475
echo NEAR_MAX
echo 5 5 1900 1140

set config=%1
if %config%nul==nul set config=normal

goto %config%

:NORMAL
echo NORMAL

:: Outlook
nircmd.exe win setsize etitle "Outlook" 30 350 1800 785

nircmd.exe win setsize etitle "Notepad++" 55 330 1750 800

nircmd.exe win setsize etitle "TortoiseHg Workbench" 80 300 1750 820

nircmd.exe win setsize etitle "Internet Explorer" 111 260 1720 860

nircmd.exe win setsize etitle "Microsoft Visual Studio" 135 100 1700 1030

nircmd.exe win setsize etitle "notepad2" 905 10 1000 1130

nircmd.exe win setsize etitle "Google Chrome" 2005 220 1820 950

nircmd.exe win setsize etitle "Excel" 2305 60 1500 1090

nircmd.exe win setsize etitle "Management Studio" 2090 160 1700 980

nircmd.exe win setsize etitle "Fuse Configuration Manager" 240 100 1640 1000

nircmd.exe win setsize etitle "Intel Enterprise PDF Reader" 2720 50 1110 1090

:REMINDER
nircmd.exe win setsize etitle "Reminder(s)" 600 30 500 360

goto:EOF

:LAPTOP
echo LAPTOP
:: Outlook
nircmd.exe win setsize etitle "Outlook" 15 170 1400 640
nircmd.exe win setsize etitle "Notepad++" 25 155 1400 650
nircmd.exe win setsize etitle "TortoiseHg Workbench" 45 130 1400 675
nircmd.exe win setsize etitle "Internet Explorer" 60 112 1400 695
nircmd.exe win setsize etitle "Google Chrome" 80 80 1400 730
nircmd.exe win setsize etitle "Microsoft Visual Studio" 120 25 1340 780
nircmd.exe win setsize etitle "Intel Enterprise PDF Reader" 640 10 890 800





