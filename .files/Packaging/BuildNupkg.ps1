<#
.Synopsis
    Builds solution and creates NuGet packages.
#>
param
(
    [Parameter(HelpMessage = "Path to the solution directory.")]
    [String] $solutionDir = '.',

    [Parameter(HelpMessage = "Path to the solution output directory.")]
    [String] $outputDir = 'Assemblies',

    [Parameter(HelpMessage = "Path to GlobalAssemblyInfo.cs.")]
    [String] $assemblyInfo = '.files\Packaging\GlobalAssemblyInfo.cs',

    [Parameter(HelpMessage = "VCS branch name.")]
    [String] $branchName = '',

    [Parameter(HelpMessage = "VCS commit hash.")]
    [String] $commitHash = '',

    [Parameter(HelpMessage = "Build mode.")]
    [String] $buildMode = 'Release'
)

# Script dependencies
. (Join-Path $PSScriptRoot 'CreateNuspec.ps1')
. (Join-Path $PSScriptRoot 'BuildSymbols.ps1')

# Clear output directory
Remove-Item -Path $outputDir -Recurse -ErrorAction SilentlyContinue

# Install NuGet package manager

$nugetDir = Join-Path $env:ProgramData 'NuGet'
$nugetPath = Join-Path $nugetDir 'nuget.exe'

if (-not (Test-Path $nugetPath))
{
    if (-not (Test-Path $nugetDir))
    {
        New-Item $nugetDir -ItemType Directory -ErrorAction SilentlyContinue
    }

    $nugetSourceUri = 'http://dist.nuget.org/win-x86-commandline/latest/nuget.exe'
    Invoke-WebRequest -Uri $nugetSourceUri -OutFile $nugetPath
}

# Restore packages
& "$nugetPath" restore 'InfinniPlatform.PrintViewDesigner.sln' -NonInteractive

# Build the solution
& "${Env:ProgramFiles(x86)}\MSBuild\14.0\bin\msbuild.exe" 'InfinniPlatform.PrintViewDesigner.sln' /t:Clean /p:Configuration=$buildMode /verbosity:quiet /consoleloggerparameters:ErrorsOnly
& "${Env:ProgramFiles(x86)}\MSBuild\14.0\bin\msbuild.exe" 'InfinniPlatform.PrintViewDesigner.sln' /p:Configuration=$buildMode /verbosity:quiet /consoleloggerparameters:ErrorsOnly

# Create nuspec-files
Create-Nuspec `
    -solutionDir $solutionDir `
    -outputDir $outputDir `
    -assemblyInfo $assemblyInfo `
    -branchName $branchName `
    -commitHash $commitHash

Get-ChildItem $outputDir -Filter '*.nuspec' | Foreach-Object {
    $nuspecFile = Join-Path $outputDir $_.Name

    # Create nupkg-file
    & "$nugetPath" pack $nuspecFile -OutputDirectory $outputDir -NoDefaultExcludes -NonInteractive

    # Remove nuspec-file
    Remove-Item -Path $nuspecFile
}