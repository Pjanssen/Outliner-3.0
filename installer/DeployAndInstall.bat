@ECHO OFF

SET CurrentDir=%~dp0

CALL %CurrentDir%\Deploy.bat
CALL %CurrentDir%\Install.bat

goto :eof

::=============================================================================

:ErrorMsg
ECHO %~1 >> %Log%
GOTO :error

:error
PAUSE
EXIT /B %ERRORLEVEL%