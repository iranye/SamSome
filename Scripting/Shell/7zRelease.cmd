@echo off

set input_dir=%1
set rev=%2
set dest_dir=%3

if %input_dir%nul==nul goto error
if %rev%nul==nul set rev=1_0
set zip_name=%input_dir%-%rev%.7z

if %dest_dir%nul==nul set dest_dir=.
if exist %zip_name% del %zip_name%

set params=-mx0 -xr!bin -xr!obj -xr!_TBD -xr!SoftwareUnderTest -xr!Testing -xr!RDL-Data -xr!*.user -xr!*.DotSettings
echo --- 7z a %zip_name% %input_dir% %params%
pause

7z a %zip_name% %input_dir% %params%
echo created: '%zip_name%'
pause
move /y %zip_name% %dest_dir%
echo moved: %zip_name% to %dest_dir%

goto:eof

:error
echo usage: %0 "<directory to zip>"
pause