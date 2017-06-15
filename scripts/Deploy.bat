@echo off

Title Deploying MediaPortal Spectrum Analyzer (RELEASE)
cd ..

if "%programfiles(x86)%XXX"=="XXX" goto 32BIT
	:: 64-bit
	set PROGS=%programfiles(x86)%
	goto CONT
:32BIT
	set PROGS=%ProgramFiles%
:CONT
IF NOT EXIST "%PROGS%\Team MediaPortal\MediaPortal\" SET PROGS=C:

copy /y "SpectrumAnalyzer\bin\Release\SpectrumAnalyzer.dll" "%PROGS%\Team MediaPortal\MediaPortal\plugins\process\"

cd scripts