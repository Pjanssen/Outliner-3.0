@ECHO OFF

SET CurrentDir=%~dp0
SET Installer="%CurrentDir%\Builds\Latest_Full.exe"
SET Log="%CurrentDir%\Logs\Install.log"

::=============================================================================

IF NOT EXIST "%CurrentDir%\Logs" MKDIR "%CurrentDir%\Logs"
IF EXIST %Log% DEL %Log% /Q

::=============================================================================

%Installer% /SP- /SILENT /LOG=%Log% /TARGETMAXVERSION 15 || GOTO :error

goto :eof

::=============================================================================

:ErrorMsg
ECHO %~1 >> %Log%
GOTO :error

:error
PAUSE
EXIT /B %ERRORLEVEL%