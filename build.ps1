$major = "3.0.0"
$pre = 1
$githash = git rev-parse --short HEAD 
$branch = git rev-parse --abbrev-ref HEAD

if ($branch -eq "master") {
  $version = "$major"
}
else {
  $version = "$major-pre$pre"
}

Write-Host $version


dotnet test

#setversion 