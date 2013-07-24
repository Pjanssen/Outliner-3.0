@ECHO OFF

SET OutlinerSolution="..\Outliner.sln"
SET Log="Logs\Build.log"

::=============================================================================

CALL :GetMsBuild MSBuild 4.0
IF "%MSBuild%"=="" goto :ErrorMsg "Could not find MSBuild.exe for .NET 4.0."
dir "%MSBuild%" > nul || goto :ErrorMsg "Could not find MSBuild.exe for .NET 4.0."

IF NOT EXIST "Logs" MKDIR "Logs"
IF EXIST %Log% DEL %Log% /Q

::=============================================================================

SET ISCMDLINEBUILD=TRUE

CALL :logSection "Compiling Outliner.Core"
%MSBuild% %OutlinerSolution% /p:Configuration=Release /nologo >> %Log% || GOTO :error

SET ISCMDLINEBUILD=FALSE

goto :eof

::=============================================================================

:: Usage: CALL :GetMSBuild variable_to_set dot_net_version
:GetMSBuild
SET KEY_NAME=HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\
SET KEY_VALUE=MSBuildToolsPath

FOR /F "tokens=2*" %%A IN ('REG QUERY "%KEY_NAME%%2" /v %KEY_VALUE% 2^>nul') DO (
   set %~1=%%B\MSBuild.exe
)
GOTO :eof

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