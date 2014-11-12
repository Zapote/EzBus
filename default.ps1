properties { 
  $ProductVersion = "0.9"
  $TargetFramework = "net-4.0"
  $BuildNumber = "2"
} 

$baseDir  = resolve-path .
$buildDir = "$baseDir\build" 
$toolsDir = "$baseDir\Tools"
$outputDir = "$buildDir\Output"
$artifactsDir = "$buildDir\Artifacts"
$releaseDir = "$buildDir\Release"
$nunitexec = "$toolsDir\nunit-2.6.3\nunit-console.exe"
$zipExec = "$toolsDir\zip\7za.exe"
$nugetExec = "$toolsDir\nuget\nuget.exe"
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
		if ($TargetFramework -eq "net-4.0"){
			$netfxInstallroot ="" 
			$netfxInstallroot =	Get-RegistryValue 'HKLM:\SOFTWARE\Microsoft\.NETFramework\' 'InstallRoot' 
			$netfxCurrent = $netfxInstallroot + "v4.0.30319"
			$script:msBuild = $netfxCurrent + "\msbuild.exe"
			
			echo ".Net 4.0 build requested - $script:msBuild" 

			$script:msBuildTargetFramework ="/p:TargetFrameworkVersion=v4.0 /ToolsVersion:4.0"
			$script:nunitTargetFramework = "/framework=4.0";
			$script:isEnvironmentInitialized = $true
		}
	}
}

task Init -depends InitEnvironment, Clean, DetectOperatingSystemArchitecture {   	
	echo "Creating build directory at the follwing path $buildDir"
	Delete-Directory $buildDir
	Create-Directory $buildDir
	Delete-Directory $releaseDir
	Create-Directory $releaseDir
	Delete-Directory $artifactsDir
	Create-Directory $artifactsDir
	
	$script:Version = $ProductVersion + "." + $BuildNumber
	$currentDirectory = Resolve-Path .
	
	echo "Current Directory: $currentDirectory" 
 }
 
task GenerateAssemblyInfo{
	$assemblyInfoDirs = Get-ChildItem -path "$baseDir" -recurse -include "*.csproj" | % {
		$propDir = $_.DirectoryName + "\Properties"
		Create-Directory $propDir
		
		$version = "$ProductVersion.$BuildNumber.0"
		
		$nuspecfile = Get-ChildItem -Path $_.DirectoryName -Filter "*.nuspec"
		if($nuspecfile -ne $null){ 
			[xml]$content = Get-Content $nuspecfile.fullname
			$version = $content.package.metadata.version + ".0"
		}
		
		Generate-Assembly-Info `
		-file "$propDir\AssemblyInfo.cs" `
		-title "$name $version" `
		-description "" `
		-company "Zapote" `
		-product "$name $version" `
		-version $version `
		-copyright "Zapote" `
	}
}

task CompileMain { 
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
	exec {&$nunitexec $testAssemblies $script:nunitTargetFramework /xml="$buildDir\TestReports\TestResults.xml" /noshadow /nologo } 
} 

task UpdateNugetPackageVersion {
    
    echo $Version
    
    dir $outputDir -recurse -include *.nuspec | % {
		$nuspecfile = $_.FullName
		
		
		[xml]$content = Get-Content $nuspecfile
		$content.package.metadata.version = $Version

        foreach( $dependency in $content.package.metadata.dependencies.dependency) { 
            if($dependency.Id.StartsWith("EzBus")){
                $dependency.version = "[" + $Version + "]";
            } 
        }

        $content.save($nuspecfile)
	}

}

task CreateNugetPackages -depends UpdateNugetPackageVersion {
	dir $outputDir -recurse -include *.nuspec | % {
		$nuspecfile = $_.FullName
		
		
		[xml]$content = Get-Content $nuspecfile
		$packageVersion = $content.package.metadata.version
				
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
