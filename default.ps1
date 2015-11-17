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

task DoRelease -depends GenerateAssemblyInfo, Test, CreateNugetPackages{
}

task Clean{
	if(Test-Path $buildDir){
		Delete-Directory $buildDir	
	}
}

task InitEnvironment{
	if($script:isEnvironmentInitialized -ne $true){
		$script:msBuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"	
		echo ".Net 4.0 build requested - $script:msBuild" 
	}
}

task Init -depends InitEnvironment, Clean, DetectOperatingSystemArchitecture {   	
	write-host "Creating build directory at the follwing path $buildDir"
	
	Create-Directory $buildDir
	Create-Directory $releaseDir
	Create-Directory $artifactsDir
	
	$currentDirectory = Resolve-Path .
	
	write-host "Current Directory: $currentDirectory" 
 }

task GitVersion{
	$script:gitVersionInfo = ( &$gitVersionExec | Out-String | ConvertFrom-Json)
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

task CompileMain -depends Init { 
	Delete-Directory $outputDir
	Create-Directory $outputDir

	Write-Host "Compiling version: $Version"

	$toExclude = @();
	$toExclude = "*nobuild.sln"
	
	$solutions = Get-ChildItem -path "$baseDir" -recurse -include *.sln -Exclude $toExclude
	$solutions | % {
		$solutionFile = $_.FullName
		$solutionName = $_.BaseName
		$solutionDir = $_.Directory
		$targetDir = "$outputDir\$solutionName\"
		
		Create-Directory $targetDir
		
		write-host $script:msBuild
		
		exec { &$script:msBuild $solutionFile /p:OutDir="$targetDir\" /p:Configuration=Release }
	}
}

task Test -depends CompileMain{	
	if(Test-Path $buildDir\TestReports){
		Delete-Directory $buildDir\TestReports
	}
	
	Create-Directory $buildDir\TestReports
	
	$testAssemblies = @()
	$testAssemblies += Get-ChildItem -path "$outputDir" -recurse -include *.Test.dll
	$targetFramework = "/framework=4.5";
	exec {&$nunitexec $testAssemblies $targetFramework /xml="$buildDir\TestReports\TestResults.xml" /noshadow /nologo } 
} 

task UpdateNugetPackageVersion {
    echo "Updating packages to version $Version"
    dir $outputDir -recurse -include *.nuspec | % {
		$nuspecfile = $_.FullName
		[xml]$content = Get-Content $nuspecfile
		$version = $script:gitVersionInfo.NuGetVersion
		$content.package.metadata.version = $version
			
		if($content.package.metadata.dependencies.dependency -ne $null){
			foreach($dependency in $content.package.metadata.dependencies.dependency) { 
				if($dependency.Id.StartsWith("EzBus")){
					$dependency.version = "[" + $version + "]";
				} 
			}
		}
		
		$content.save($nuspecfile)
	}

}

task CreateNugetPackages -depends UpdateNugetPackageVersion{
    
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
