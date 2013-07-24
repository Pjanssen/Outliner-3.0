@ECHO OFF

CALL Build.cmd || GOTO :eof
CALL Deploy.cmd || GOTO :eof
CALL Install.cmd