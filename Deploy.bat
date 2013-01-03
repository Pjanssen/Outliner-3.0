@ECHO OFF

SET CurrentDir=%~dp0
SET InnoSetupCompiler="C:\Program Files (x86)\Inno Setup 5\iscc.exe"
SET SigCheck=%CurrentDir%\sigcheck.exe
SET CoreAssembliesSource="%CurrentDir%\..\Outliner.Core\Deployment"
SET CoreAssembliesTarget="%CurrentDir%\Deployment\assemblies"
SET PluginsSource="%CurrentDir%\..\Outliner.Plugins"
SET PluginsTarget="%CurrentDir%\Deployment\plugcfg\Plugins"
SET SetupOutput="%CurrentDir%\Builds"
SET LogDir="%CurrentDir%\Logs"
SET Log="%LogDir%\Deploy.log"

::=============================================================================
IF NOT EXIST "%LogDir%" MKDIR "%LogDir%"
IF EXIST %Log% DEL %Log% /Q

::=============================================================================

CALL :LogSection "Copying Core assemblies"
IF NOT EXIST %CoreAssembliesTarget% MKDIR "%CoreAssembliesTarget%"
DEL "%CoreAssembliesTarget%\*.*" /Q >> %Log%
XCOPY "%CoreAssembliesSource%\*.dll" "%CoreAssembliesTarget%" >> %Log% || GOTO :error

CALL :LogBlank
CALL :LogSection "Copying plugin assemblies"

IF NOT EXIST %PluginsTarget% MKDIR "%PluginsTarget%"
DEL "%PluginsTarget%\*.*" /Q >> %Log%
FOR /D %%P IN ("%CurrentDir%\..\Outliner.Plugins\*") DO (
    ECHO %%P >> %Log%
    XCOPY "%%P\bin\release\*.dll" "%PluginsTarget%\" /Y >> %Log% || GOTO :error
)
::=============================================================================

CALL :LogBlank
CALL :LogSection "Determining Core version"
for /f %%i in ('%SigCheck% -n -q "%CoreAssembliesTarget%\Outliner.Core.dll"') do SET FullCoreVersion=%%i
ECHO %FullCoreVersion% >> %Log%
for /f "tokens=1,2,3,4 delims=." %%a in ("%FullCoreVersion%") do SET ShortCoreVersion=%%a.%%b.%%c
ECHO %ShortCoreVersion% >> %Log%

::=============================================================================

CALL :LogBlank
CALL :LogSection "Compiling regular installer"
%InnoSetupCompiler% "%CurrentDir%\regular.iss" /O"%SetupOutput%" /F"Outliner_%ShortCoreVersion%_Regular" >> %Log% || GOTO :error

CALL :LogBlank
CALL :LogSection "Compiling full installer"
%InnoSetupCompiler% "%CurrentDir%\full.iss" /O"%SetupOutput%" /F"Outliner_%ShortCoreVersion%_Full" >> %Log% || GOTO :error

::=============================================================================

CALL :LogBlank
CALL :LogSection "Copying latest version"

COPY "%SetupOutput%\Outliner_%ShortCoreVersion%_Full.exe" "%SetupOutput%\Latest_Full.exe" /Y >> %Log% || GOTO :error
COPY "%SetupOutput%\Outliner_%ShortCoreVersion%_Regular.exe" "%SetupOutput%\Latest_Regular.exe" /Y >> %Log% || GOTO :error

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