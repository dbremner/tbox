@echo off
%WinDir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe "%CD%\Library.sln" /t:rebuild /p:Configuration="Release"
pause