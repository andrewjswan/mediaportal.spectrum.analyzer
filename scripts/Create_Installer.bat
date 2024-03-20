@echo off
cls
Title Creating MediaPortal Spectrum Analyzer Installer

if "%programfiles(x86)%XXX"=="XXX" goto 32BIT
	:: 64-bit
	set PROGS=%programfiles(x86)%
	goto CONT
:32BIT
	set PROGS=%ProgramFiles%
:CONT

IF NOT EXIST "%PROGS%\Team MediaPortal\MediaPortal\" SET PROGS=C:

:: Get version from DLL
FOR /F "tokens=*" %%i IN ('Tools\sigcheck.exe /accepteula /nobanner /n "..\SpectrumAnalyzer\bin\Release\SpectrumAnalyzer.dll"') DO (SET version=%%i)

:: Temp xmp2 file
copy SpectrumAnalyzer.xmp2 SpectrumAnalyzerTemp.xmp2

:: Sed "{VERSION}" from xmp2 file
Tools\sed.exe -i "s/{VERSION}/%version%/g" SpectrumAnalyzerTemp.xmp2

:: Build mpe1
"%PROGS%\Team MediaPortal\MediaPortal\MPEMaker.exe" SpectrumAnalyzerTemp.xmp2 /B /V=%version% /UpdateXML

:: Cleanup
del SpectrumAnalyzerTemp.xmp2

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
