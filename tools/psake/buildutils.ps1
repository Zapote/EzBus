
function Delete-Directory($directoryName){
	if(Test-Path($directoryName)){
		Remove-Item -Force -Recurse $directoryName -ErrorAction SilentlyContinue
	}
}
 
function Create-Directory($directoryName){
	New-Item $directoryName -ItemType Directory | Out-Null
}

function AddType{
	Add-Type -TypeDefinition "
	using System;
	using System.Runtime.InteropServices;
	public static class Win32Api
	{
	    [DllImport(""Kernel32.dll"", EntryPoint = ""IsWow64Process"")]
	    [return: MarshalAs(UnmanagedType.Bool)]
	    public static extern bool IsWow64Process(
	        [In] IntPtr hProcess,
	        [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process
	    );
	}
	"
}
 
function Generate-Assembly-Info{

param(
	[string]$assemblyTitle,
	[string]$assemblyDescription,
	[string]$clsCompliant = "true",
	[string]$internalsVisibleTo = "",
	[string]$configuration, 
	[string]$company, 
	[string]$product, 
	[string]$copyright, 
	[string]$version,
	[string]$fileVersion,
	[string]$infoVersion,	
	[string]$file = $(throw "file is a required parameter.")
)
	if($infoVersion -eq ""){
		$infoVersion = $fileVersion
	}

	$asmInfo = "using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(""$assemblyTitle"")]
[assembly: AssemblyVersion(""$version"")]
[assembly: AssemblyFileVersion(""$version"")]
[assembly: AssemblyProduct(""$product"")]
[assembly: AssemblyInformationalVersion(""$infoVersion"")]
[assembly: ComVisible(false)]		
"
	
	if($clsCompliant.ToLower() -eq "true"){
		 $asmInfo += "[assembly: CLSCompliantAttribute($clsCompliant)]"
	} 
	
	if($internalsVisibleTo -ne ""){
		$asmInfo += "[assembly: InternalsVisibleTo(""$internalsVisibleTo"")]"	
	}
	
	$dir = [System.IO.Path]::GetDirectoryName($file)
	
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}

	write-host "Generating assembly info file: $file"
	Write-Output $asmInfo > $file
}