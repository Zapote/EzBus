$baseDir  = resolve-path .
$srcDir = "$baseDir\src"
$buildDir = "$baseDir\build" 
$toolsDir = "$baseDir\Tools"
$outputDir = "$buildDir\output"
$artifactsDir = "$buildDir\artifacts"
$releaseDir = "$buildDir\release"
$reportsDir = "$buildDir\reports"

$gitVersionExec = "$toolsDir\gitversion\GitVersion.exe"
include $toolsDir\psake\buildutils.ps1

task default -depends DoRelease

task DoRelease -depends Test, Pack

task Init{   	
	Delete-Directory $buildDir	

	write-host "Creating build directory at the follwing path $buildDir"

	Create-Directory $buildDir
	Create-Directory $outputDir
	Create-Directory $releaseDir
	Create-Directory $artifactsDir
	
	$currentDirectory = Resolve-Path .
	
	write-host "Current Directory: $currentDirectory" 
 }

task GitVersion{
	$script:gitVersionInfo = ( &$gitVersionExec | Out-String | ConvertFrom-Json)
	$buildVersion = $script:gitVersionInfo.NuGetVersion 
	Write-Host "##teamcity[buildNumber '$buildVersion']"
}
 
task GenerateAssemblyInfo -depends GitVersion{
	$version = $script:gitVersionInfo.AssemblySemVer
	$informationalVersion = $gitVersionInfo.InformationalVersion
	
	$assemblyInfoDirs = Get-ChildItem -path "$baseDir" -recurse -include "*.csproj" | % {
		$propDir = $_.DirectoryName + "\Properties"
		Create-Directory $propDir
		
		$name = $_.Basename
		
		Generate-Assembly-Info `
		-file "$propDir\AssemblyInfo.cs" `
		-title "$name" `
		-description "" `
		-company "Zapote" `
		-version $version `
		-informationalVersion "$informationalVersion" `
		-copyright "None ©" `
	}
}

task Build -depends Init { 
	$projects = gci -path "$srcDir" -recurse -include *.csproj
	
	$projects | % {
		$projectFile = $_.FullName
		$projectName = $_.BaseName

		[xml]$projectXml = Get-Content -Path $projectFile
		$sdk = $projectXml.Project.Sdk

		if($sdk -eq "Microsoft.NET.Sdk"){
			# multi targets
			
			$targetFrameworks = $projectXml.Project.PropertyGroup.TargetFrameworks
		
			# single targets
			if(!$targetFrameworks){
				$targetFrameworks = $projectXml.Project.PropertyGroup.TargetFramework
			}
		} else {
			$targetFrameworks = "net46"
		}
		
		write-host "Build $projectName"
		
		$targetFrameworks.Split(";") | % {
			$targetFramework = $_
			write-host "Building $projectName for $targetFramework"
			exec { dotnet restore $projectFile --no-cache -v q }
			exec { dotnet build $projectFile -c Release -o "$outputDir\$projectName\$targetFramework"  -f $targetFramework }
		}
		
	}

	write-host "build done"
}

task Test -depends Build {	
	Delete-Directory $reportsDir
	Create-Directory $reportsDir
	$tests = gci -path "$outputDir" -recurse -include *Test.dll 
	$testReport = "$reportsDir\test-report.trx"
	exec { dotnet vstest $tests --"logger:trx;LogFileName=$testReport" }
} 

task Pack -depends Build {	
	Delete-Directory $artifactsDir
	Create-Directory $artifactsDir
	gci -path "$srcDir" -recurse -include *.csproj | % {
		exec { dotnet pack $_ -o $artifactsDir --no-build -c Release }
	}
} 

task CreateNugetPackages {
	dir $outputDir -recurse -include *.nuspec | % {
		$nuspecfile = $_.FullName
		
		[xml]$content = Get-Content $nuspecfile
		$packageVersion = $script:gitVersionInfo.NuGetVersion
				
        exec { &$nugetExec pack $nuspecfile -OutputDirectory $artifactsDir -Version $packageVersion }
	}
}

