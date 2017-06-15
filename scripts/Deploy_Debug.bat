@echo off
cls
Title Deploying MediaPortal Spectrum Analyzer (DEBUG)
cd ..

if "%programfiles(x86)%XXX"=="XXX" goto 32BIT
	:: 64-bit
	set PROGS=%programfiles(x86)%
	goto CONT
:32BIT
	set PROGS=%ProgramFiles%
:CONT

copy /y "SpectrumAnalyzer\bin\Debug\SpectrumAnalyzer.dll" "%PROGS%\Team MediaPortal\MediaPortal\plugins\process\"
cd scripts
