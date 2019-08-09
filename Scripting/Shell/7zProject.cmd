@echo off

set input_dir=%1
set dest_dir=%2

if %input_dir%nul==nul goto error

set massagedTime=%TIME: =0%
set HH=%massagedTime:~0,2%
set timestamp=%date:~-4%%date:~4,2%%date:~7,2%-%HH%%time:~3,2%%time:~6,2%

set zip_name=%input_dir%_%timestamp%.7z

echo dest_dir=%dest_dir%
pause

if exist %zip_name% del %zip_name%

if %dest_dir%nul==nul set dest_dir=%backups%
if exist %dest_dir%\NUL goto start

if %backups%nul==nul set dest_dir=.
echo Backups directory not yet set.  Using default option...
echo .
pause

:start
7z a %zip_name% %input_dir% -mx0 -xr!bin -xr!obj -xr!_TBD -xr!SoftwareUnderTest -xr!packages -xr!.hg -xr!_DATA -p -xr!.vs
echo created: '%zip_name%'
echo .
echo moving to '%dest_dir%'
pause

move /y %zip_name% %dest_dir%
echo moved: %zip_name% to %dest_dir%

goto:eof

:error
echo usage: %0 "<directory to zip>"
pause