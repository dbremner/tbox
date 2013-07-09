@echo off
%1 %2 %3

if %ERRORLEVEL%==0 exit
echo Build failed!
pause