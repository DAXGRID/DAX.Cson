@echo off

set version=%1
set currentdir=%~dp0
set root=%currentdir%

set destination=%root%\deploy
set msbuild=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MsBuild.exe
set nuget=%root%\tools\NuGet\nuget.exe

set solutionfile=%root%\NRGi.Cson.sln
set nuspec=%currentdir%\NRGi.Cson\NRGi.Cson.nuspec

if "%version%"=="" (
	echo Please remember to specify which version to build as an argument to this script.
	echo.
	goto exit
)


"%msbuild%" "%solutionfile%" /p:Configuration=Release

"%nuget%" pack "%nuspec%" -OutputDirectory "%destination%" -Version "%version%"


:exit