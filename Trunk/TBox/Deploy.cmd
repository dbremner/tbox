@echo off

set source=bin\Release
set target=c:\data\1\Releases\TBox\
set dataFolder=%target%\Data
set toolsFolder=%target%\Tools
RD /S /Q %target%
MD %target%

md %target%\Libraries
copy %source%\Libraries\*.dll  %target%\Libraries
copy %source%\Libraries\*.xml  %target%\Libraries

md %target%\Plugins
copy %source%\Plugins\*.dll  %target%\Plugins
copy %source%\Plugins\*.xml  %target%\Plugins
del %target%\Plugins\marketclient.dll

md %target%\Themes
copy %source%\Themes\*.xaml  %target%\Themes

md %toolsFolder%
copy Tools\*.exe  %toolsFolder%
copy Tools\*.cmd  %toolsFolder%

md %dataFolder%
md %dataFolder%\Automater
xcopy Shared\Automater\*.* %dataFolder%\Automater\ /s /i

copy %source%\*.txt %target%
copy %source%\*.config %target%
del %target%\Config.config
copy %source%\*.dll %target%
copy %source%\*.exe %target%
del %target%\*vshost.*

if %ERRORLEVEL%==0 goto script_end
echo Deploy failed!
pause

:script_end