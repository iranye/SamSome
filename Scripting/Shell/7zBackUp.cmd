@echo off

set input_dir=%1
set dest_dir=%2

if %input_dir%nul==nul goto error
7z a %input_dir%.7z %input_dir% -mx0 -xr!bin -xr!obj -xr!_TBD -xr!SoftwareUnderTest -p 
echo created: '%input_dir%.7z'

if %dest_dir%nul==nul set dest_dir=%backups%

move /y %input_dir%.7z %dest_dir%
goto:eof

:error
echo usage: %0 "<directory to zip>"
pause