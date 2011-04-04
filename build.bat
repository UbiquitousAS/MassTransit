@echo off

echo Building for .NET 3.5
call rake BUILD_CONFIG_KEY=NET35

rem echo Building for .NET 4.0
rem call rake unclean

echo Creating NU package
lib\nuget pack MassTransit.nuspec /OutputDirectory build_artifacts 

echo Create ZIP package
call rake package
