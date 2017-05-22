$baseDir  = resolve-path .
$buildDir = "$baseDir\build" 
$toolsDir = "$baseDir\Tools"
$outputDir = "$buildDir\Output"
$artifactsDir = "$buildDir\Artifacts"
$releaseDir = "$buildDir\Release"
$nunitexec = "$toolsDir\nunit-2.6.3\nunit-console.exe"
$zipExec = "$toolsDir\zip\7za.exe"
$nugetExec = "$toolsDir\nuget\nuget.exe"
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

	$toExclude = @();
	$toExclude = "*nobuild.sln"
	
	$solutions = Get-ChildItem -path "$baseDir" -recurse -include *EzBus-core.sln -Exclude $toExclude
	$solutions | % {
		$solutionFile = $_.FullName
		$solutionName = $_.BaseName
		exec { dotnet build $solutionFile -c Release -v q}
	}
}

task Test -depends Build{	
	gci -path "$srcDir" -recurse -include *Test.csproj | % {
		$projectFile = $_.FullName
		$projectName = $_.BaseName
		exec { dotnet test $projectFile -v q --no-build}
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

task DetectOperatingSystemArchitecture {
	if (IsWow64 -eq $true)
	{
		$script:architecture = "x64"
	}
    echo "Machine Architecture is $script:architecture"
 }
