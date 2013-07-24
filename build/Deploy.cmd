@ECHO OFF

SET InnoSetupCompiler="C:\Program Files (x86)\Inno Setup 5\iscc.exe"
SET SigCheck=sigcheck.exe
SET InstallerSourceDir=..\Installer
SET CoreAssembly=Sources\assemblies\Outliner.Core.dll
SET SetupOutput=Installers
SET LogDir=Logs\
SET Log=%LogDir%Deploy.log

::=============================================================================

IF NOT EXIST %LogDir% MKDIR %LogDir%
IF EXIST %Log% DEL %Log% /Q
::=============================================================================

CALL :LogBlank
CALL :LogSection "Determining Core version"
for /f %%i in ('%SigCheck% -n -q "%CoreAssembly%"') do SET FullCoreVersion=%%i
ECHO %FullCoreVersion% >> %Log%
for /f "tokens=1,2,3,4 delims=." %%a in ("%FullCoreVersion%") do SET ShortCoreVersion=%%a.%%b.%%c
ECHO %ShortCoreVersion% >> %Log%

::=============================================================================

CALL :LogBlank
CALL :LogSection "Compiling full installer"
%InnoSetupCompiler% "%InstallerSourceDir%\full.iss" /O"%SetupOutput%" /F"Outliner_%ShortCoreVersion%_Full" >> %Log% || GOTO :error
COPY "%SetupOutput%\Outliner_%ShortCoreVersion%_Full.exe" "%SetupOutput%\Latest_Full.exe" /Y >> %Log% || GOTO :error

CALL :LogBlank
CALL :LogSection "Compiling regular installer"
%InnoSetupCompiler% "%InstallerSourceDir%\regular.iss" /O"%SetupOutput%" /F"Outliner_%ShortCoreVersion%_Regular" >> %Log% || GOTO :error
COPY "%SetupOutput%\Outliner_%ShortCoreVersion%_Regular.exe" "%SetupOutput%\Latest_Regular.exe" /Y >> %Log% || GOTO :error

::=============================================================================

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