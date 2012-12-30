@ECHO OFF

SET InnoSetupCompiler="C:\Program Files (x86)\Inno Setup 5\iscc.exe"
SET CoreAssembliesSource="..\Outliner.Core\Outliner.Core\bin\release"
SET CoreAssembliesTarget="Deployment\assemblies"
SET PluginsSource="..\Outliner.Plugins"
SET PluginsTarget="Deployment\plugcfg\Plugins"
SET Log="Logs\Deploy.log"

::=============================================================================

IF NOT EXIST "Logs" MKDIR "Logs"
IF EXIST %Log% DEL %log% /Q

IF EXIST "Output" DEL "Output\*.*" /Q

::=============================================================================

CALL :LogSection "Copying Core assemblies"
IF NOT EXIST %CoreAssembliesTarget% MKDIR "%CoreAssembliesTarget%"
DEL "%CoreAssembliesTarget%\*.*" /Q >> %Log%
XCOPY "%CoreAssembliesSource%\*.dll" "%CoreAssembliesTarget%" >> %Log% || GOTO :error

CALL :LogBlank
CALL :LogSection "Copying plugin assemblies"

IF NOT EXIST %PluginsTarget% MKDIR "%PluginsTarget%"
DEL "%PluginsTarget%\*.*" /Q >> %Log%
FOR /D %%P IN ("..\Outliner.Plugins\*") DO (
    ECHO %%P >> %Log%
    XCOPY "%%P\bin\release\*.dll" "%PluginsTarget%\" /Y >> %Log% || GOTO :error
)

::=============================================================================

CALL :LogBlank
CALL :LogSection "Determining Core version"
for /f %%i in ('sigcheck -n -q "%CoreAssembliesTarget%\Outliner.Core.dll"') do SET FullCoreVersion=%%i
ECHO %FullCoreVersion% >> %Log%
for /f "tokens=1,2,3,4 delims=." %%a in ("%FullCoreVersion%") do SET ShortCoreVersion=%%a.%%b.%%c
ECHO %ShortCoreVersion% >> %Log%

::=============================================================================

CALL :LogBlank
CALL :LogSection "Compiling regular installer"
%InnoSetupCompiler% "regular.iss" /F"OutlinerSetup_%ShortCoreVersion%_Regular" >> %Log% || GOTO :error

CALL :LogBlank
CALL :LogSection "Compiling full installer"
%InnoSetupCompiler% "full.iss" /F"OutlinerSetup_%ShortCoreVersion%_Full" >> %Log% || GOTO :error

goto :eof

::=============================================================================

:LogBlank
ECHO. >> %Log%
ECHO. >> %Log%
GOTO :eof

:LogSection
ECHO ============================================================== >> %Log%
ECHO %~1 >> %Log%
ECHO ============================================================== >> %Log%
ECHO. >> %Log%
GOTO :eof

:ErrorMsg
ECHO %~1 >> %Log%
GOTO :error

:error
PAUSE
EXIT /B %ERRORLEVEL%