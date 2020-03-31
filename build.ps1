$major = "3.0.0"
$pre = 2
$branch = git rev-parse --abbrev-ref HEAD

if ($branch -eq "master") {
  $version = "$major"
}
else {
  $version = "$major-pre$pre"
}

setversion $version "./EzBus/EzBus.csproj"
setversion $version "./EzBus.Core/EzBus.Core.csproj"
setversion $version "./EzBus.RabbitMQ/EzBus.RabbitMQ.csproj"

dotnet test "./EzBus.sln" -c Release
dotnet pack "./EzBus.Core/EzBus.Core.csproj" -c Release -p:NuspecFile="ezbus.nuspec"
dotnet pack "./EzBus.RabbitMQ/EzBus.RabbitMQ.csproj" -c Release -p:NuspecFile="ezbus.rabbitmq.nuspec"
