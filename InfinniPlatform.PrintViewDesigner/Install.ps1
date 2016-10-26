<#
.Synopsis
    Installs InfinniPlatform.PrintViewDesigner.
#>
param
(
    [Parameter(HelpMessage = "Version of InfinniPlatform.PrintViewDesigner. The latest version will install by default.")]
    [String] $version = ''
)

# Install NuGet package manager

Write-Host 'Install NuGet package manager'

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

# Find InfinniPlatform.PrintViewDesigner version

Write-Host 'Find InfinniPlatform.PrintViewDesigner version'

$sources = 'https://api.nuget.org/v3/index.json;http://nuget.org/api/v2;http://nuget.infinnity.ru/api/v2'

if (-not $version)
{
    $version = (((& "$nugetPath" list 'InfinniPlatform.PrintViewDesigner' -NonInteractive -Prerelease -Source $sources) | Out-String) -split '[\r\n]' `
                | Where { $_ -match 'InfinniPlatform.PrintViewDesigner' } `
                | Select-Object -First 1) -split '\s' `
                | Select-Object -Last 1
}

Write-Host "InfinniPlatform.PrintViewDesigner.$version"

# Create install directory

$outputDir = Join-Path '.' "InfinniPlatform.PrintViewDesigner.$version"

if (Test-Path $outputDir)
{
    Write-Host "InfinniPlatform.PrintViewDesigner.$version is already installed"

    return
}

New-Item $outputDir -ItemType Directory -ErrorAction SilentlyContinue | Out-Null

# Install InfinniPlatform.PrintViewDesigner package

Write-Host "Install InfinniPlatform.PrintViewDesigner.$version"

& "$nugetPath" install 'InfinniPlatform.PrintViewDesigner' -Version $version -OutputDirectory 'packages' -NonInteractive -Prerelease -Source $sources

# Copy all references

Write-Host "Copy files"

$projectRefs = (Get-ChildItem -Path 'packages' -Filter 'InfinniPlatform.PrintViewDesigner.references' -Recurse | Select-Object -First 1)

Get-Content $projectRefs.FullName | Foreach-Object {
    if ($_ -match '^.*?\\lib(\\.*?){0,1}\\(?<path>.*?)$')
    {
        $item = Join-Path (Join-Path "$outputDir" 'bin') $matches.path

        $itemParent = Split-Path $item

        if (-not (Test-Path $itemParent))
        {
            New-Item $itemParent -ItemType Directory | Out-Null
        }

        Copy-Item -Path (Join-Path 'packages' $_) -Destination $item -Recurse -ErrorAction SilentlyContinue
    }
}
    
Copy-Item -Path (Join-Path $projectRefs.Directory.FullName "*") -Destination $outputDir -Exclude @( '*.ps1', '*references' ) -Recurse -ErrorAction SilentlyContinue

# Remove temp files

Remove-Item -Path 'packages' -Recurse -ErrorAction SilentlyContinue
