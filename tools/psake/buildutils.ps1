
function Delete-Directory($directoryName){
	if(Test-Path($directoryName)){
		Remove-Item -Force -Recurse $directoryName -ErrorAction SilentlyContinue
	}
}
 
function Create-Directory($directoryName){
	if(Test-Path($directoryName)) {return}
	New-Item $directoryName -ItemType Directory | Out-Null
}

function Get-RegistryValues($key) {
  (Get-Item $key -ErrorAction SilentlyContinue).GetValueNames()
}

function Get-RegistryValue($key, $value) {
    (Get-ItemProperty $key $value -ErrorAction SilentlyContinue).$value
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

function Is64BitOS{
    return (Test-64BitProcess) -or (Test-Wow64)
}

function Is64BitProcess{
    return [IntPtr]::Size -eq 8
}

function IsWow64{
    if ([Environment]::OSVersion.Version.Major -eq 5 -and 
        [Environment]::OSVersion.Version.Major -ge 1 -or 
        [Environment]::OSVersion.Version.Major -ge 6)
    {
		AddType
        $process = [System.Diagnostics.Process]::GetCurrentProcess()
        
        $wow64Process = $false
        
        if ([Win32Api]::IsWow64Process($process.Handle, [ref]$wow64Process) -eq $true)
        {
            return $true
        }
		else
		{
			return $false
		}
    }
    else
    {
        return $false
    }
}
 
function Generate-Assembly-Info
{
param(
	[string]$title, 
	[string]$description, 
	[string]$company, 
	[string]$product, 
	[string]$copyright, 
	[string]$version,
	[string]$informationalVersion,
	[string]$file = $(throw "file is a required parameter.")
)
  $commit = "N/A"
  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(""$title"")]
[assembly: AssemblyDescription(""$description"")]
[assembly: AssemblyCompany(""$company"")]
[assembly: AssemblyProduct(""$product"")]
[assembly: AssemblyCopyright(""$copyright"")]
[assembly: AssemblyVersion(""$version"")]
[assembly: AssemblyInformationalVersion(""$informationalVersion"")]
[assembly: AssemblyFileVersion(""$version"")]
[assembly: AssemblyDelaySign(false)]
"

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	
	Write-Output $asmInfo > $file
}

function Get-Directory([string[]]$path, [string[]]$include, [switch]$recurse) 
{ 
    Get-ChildItem -Path $path -Include $include -Recurse:$recurse | `
         Where-Object { $_.PSIsContainer } 
} 