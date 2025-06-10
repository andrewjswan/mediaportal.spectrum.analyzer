@echo off

if "%programfiles(x86)%XXX"=="XXX" goto 32BIT
    :: 64-bit
    set PROGS=%programfiles(x86)%
    goto CONT
:32BIT
    set PROGS=%ProgramFiles%
:CONT

if exist SpectrumAnalyzer_UNMERGED.dll del SpectrumAnalyzer_UNMERGED.dll
ren SpectrumAnalyzer.dll SpectrumAnalyzer_UNMERGED.dll 
ilmerge.exe /out:SpectrumAnalyzer.dll SpectrumAnalyzer_UNMERGED.dll Nlog.dll /target:dll /targetplatform:"v4,%PROGS%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2" /wildcards
