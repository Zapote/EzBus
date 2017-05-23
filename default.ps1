$baseDir  = resolve-path .
$buildDir = "$baseDir\build" 
$toolsDir = "$baseDir\Tools"
$outputDir = "$buildDir\output"
$artifactsDir = "$buildDir\artifacts"
$releaseDir = "$buildDir\release"
$reportsDir = "$buildDir\reports"
$gitVersionExec = "$toolsDir\gitversion\GitVersion.exe"
include $toolsDir\psake\buildutils.ps1

task default -depends DoRelease

task DoRelease -depends Test, CreateNugetPackages{
}

task Clean{
	if(Test-Path $buildDir){
		Delete-Directory $buildDir	
	}
}

task Init -depends Clean{   	
	write-host "Creating build directory at the follwing path $buildDir"
	
	Create-Directory $buildDir
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
	Delete-Directory $outputDir
	Create-Directory $outputDir

	$solutions = Get-ChildItem -path "$baseDir" -recurse -include EzBus.sln
	$solutions | % {
		$solutionFile = $_.FullName
		$solutionName = $_.BaseName
		
		dotnet restore $solutionFile 
		dotnet build $solutionFile -c Release -v q -o "$outputDir\$solutionName"
	}
}

task Test -depends Build{	
	Delete-Directory $reportsDir
	Create-Directory $reportsDir
	$tests = gci -path "$outputDir" -recurse -include *Test.dll 
	$testReport = "$reportsDir\test-report.trx"
	dotnet vstest $tests --"logger:trx;LogFileName=$testReport"
} 

task CreateNugetPackages {
	dir $outputDir -recurse -include *.nuspec | % {
		$nuspecfile = $_.FullName
		
		[xml]$content = Get-Content $nuspecfile
		$packageVersion = $script:gitVersionInfo.NuGetVersion
				
        exec { &$nugetExec pack $nuspecfile -OutputDirectory $artifactsDir -Version $packageVersion }
	}
}

task DetectOperatingSystemArchitecture {
	if (IsWow64 -eq $true)
	{
		$script:architecture = "x64"
	}
    echo "Machine Architecture is $script:architecture"
 }
