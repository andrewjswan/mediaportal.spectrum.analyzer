@echo off
cls
Title Creating MediaPortal Spectrum Analyzer Installer

:: Check for modification
REM svn status ..\source | findstr "^M"
REM if ERRORLEVEL 1 (
REM 	echo No modifications in source folder.
REM ) else (
REM 	echo There are modifications in source folder. Aborting.
REM 	pause
REM 	exit 1
REM )

if "%programfiles(x86)%XXX"=="XXX" goto 32BIT
	:: 64-bit
	set PROGS=%programfiles(x86)%
	goto CONT
:32BIT
	set PROGS=%ProgramFiles%
:CONT

IF NOT EXIST "%PROGS%\Team MediaPortal\MediaPortal\" SET PROGS=C:

:: Get version from DLL
FOR /F "tokens=1-3" %%i IN ('Tools\sigcheck.exe "..\SpectrumAnalyzer\bin\Release\SpectrumAnalyzer.dll"') DO ( IF "%%i %%j"=="File version:" SET version=%%k )

:: trim version
SET version=%version:~0,-1%

:: Temp xmp2 file
copy SpectrumAnalyzer.xmp2 SpectrumAnalyzerTemp.xmp2

:: Sed "SpectrumAnalyzer-{VERSION}.xml" from xmp2 file
Tools\sed.exe -i "s/SpectrumAnalyzer-{VERSION}.xml/SpectrumAnalyzer-%version%.xml/g" SpectrumAnalyzerTemp.xmp2

:: Build mpe1
"%PROGS%\Team MediaPortal\MediaPortal\MPEMaker.exe" SpectrumAnalyzerTemp.xmp2 /B /V=%version% /UpdateXML

:: Cleanup
del SpectrumAnalyzerTemp.xmp2

:: Sed "SpectrumAnalyzer-{VERSION}.mpe1" from SpectrumAnalyzer.xml
Tools\sed.exe -i "s/SpectrumAnalyzer-{VERSION}.mpe1/SpectrumAnalyzer-%version%.mpe1/g" SpectrumAnalyzer-%version%.xml

:: Parse version (Might be needed in the futute)
FOR /F "tokens=1-4 delims=." %%i IN ("%version%") DO ( 
	SET major=%%i
	SET minor=%%j
	SET build=%%k
	SET revision=%%l
)

:: Rename mpe1
if exist "..\builds\SpectrumAnalyzer-%major%.%minor%.%build%.%revision%.mpe1" del "..\builds\SpectrumAnalyzer-%major%.%minor%.%build%.%revision%.mpe1"
rename ..\builds\SpectrumAnalyzer-MAJOR.MINOR.BUILD.REVISION.mpe1 "SpectrumAnalyzer-%major%.%minor%.%build%.%revision%.mpe1"


