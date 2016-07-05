<#
.Synopsis
	Installs InfinniPlatform.PrintViewDesigner.
#>

# Package directory
$package = (Get-Item $PSScriptRoot).Parent.Parent

# Create Install directory

$outputDir = Join-Path $package.Parent.Parent.FullName $package.Name

if (Test-Path $outputDir)
{
	return
}

New-Item $outputDir -ItemType Directory -ErrorAction SilentlyContinue | Out-Null

# Find files to install
$references = Get-ChildItem -Path $package.Parent.FullName -Filter '*.references' -Recurse

# Copy all references

foreach ($projectRefs in $references)
{
	Get-Content $projectRefs.FullName | Foreach-Object {
		if ($_ -match '^.*?\\lib(\\.*?){0,1}\\(?<path>.*?)$')
		{
			$item = Join-Path "$outputDir" $matches.path

			$itemParent = Split-Path $item

			if (-not (Test-Path $itemParent))
			{
				New-Item $itemParent -ItemType Directory
			}

			Copy-Item -Path (Join-Path $package.Parent.FullName $_) -Destination $item -Recurse -ErrorAction SilentlyContinue
		}
	}
	
	Copy-Item -Path (Join-Path $projectRefs.Directory.FullName "*") -Destination $outputDir -Exclude @( '*.ps1', '*references' ) -Recurse -ErrorAction SilentlyContinue
}