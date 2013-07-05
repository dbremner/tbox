@echo off
if "%3"=="" ( %1 %2 ) ELSE ( %1 %2 /m /t:rebuild /p:Configuration=%3 ) 

if %ERRORLEVEL%==0 exit
echo Build failed!
pause