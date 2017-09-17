@echo off

set input_dir=%1
set dest_dir=%2

if %input_dir%nul==nul goto error
7z a %input_dir%.7z %input_dir%
echo created: '%input_dir%.7z'

if %dest_dir%nul==nul goto:eof

move %input_dir%.7z %dest_dir%
goto:eof

:error
echo usage: %0 "<directory to zip>"
pause