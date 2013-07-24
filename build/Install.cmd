@ECHO OFF

SET TargetMaxVersion=15
SET Installer=Installers\Latest_Full.exe
SET LogDir=Logs\
SET Log=%LogDir%Install.log

::=============================================================================

IF NOT EXIST %LogDir% MKDIR %LogDir%
IF EXIST %Log% DEL %Log% /Q

::=============================================================================

%Installer% /SP- /SILENT /LOG=%Log% /TARGETMAXVERSION %TargetMaxVersion% || GOTO :error

goto :eof

::=============================================================================

:ErrorMsg
ECHO %~1 >> %Log%
GOTO :error

:error
PAUSE
EXIT /B %ERRORLEVEL%