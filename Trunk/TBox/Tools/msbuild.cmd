@echo off
%1 %2 /m /t:rebuild /p:Configuration=%3
if %ERRORLEVEL%==0 exit
echo Build failed!
pause