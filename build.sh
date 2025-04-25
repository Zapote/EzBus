#!/bin/bash
# This script is used to build the EzBus project and run tests.
# It assumes that the .NET SDK is installed and available in the PATH.
# Usage: ./build.sh
# Check if .NET SDK is installed

if ! command -v dotnet &> /dev/null
then
    echo ".NET SDK could not be found. Please install it from https://dotnet.microsoft.com/download"
    exit 1
fi

echo " ███████╗███████╗██████╗ ██╗   ██╗██╗   ██╗███████╗
 ██╔════╝██╔════╝██╔══██╗██║   ██║██║   ██║██╔════╝
 █████╗  █████╗  ██████╔╝██║   ██║██║   ██║█████╗  
 ██╔══╝  ██╔══╝  ██╔══██╗██║   ██║██║   ██║██╔══╝  
 ███████╗███████╗██║  ██║╚██████╔╝╚██████╔╝███████╗
 ╚══════╝╚══════╝╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚══════╝
                    E Z   B U S
"

version=4.0.0
branch=$(git rev-parse --abbrev-ref HEAD)

echo current branch $branch
# Check if the current branch is 'main'
if [ "$branch" != "main" ]; then
    cc=$(git rev-list --count $branch)
    version="$version-beta-$branch-$cc"
fi

echo "Building the project version $version..."
dotnet restore "./EzBus.sln"
dotnet build "./EzBus.sln" -c Release
echo "Running tests..."
dotnet test "./EzBus.sln" -c Release
echo "Packing the project..."
dotnet pack "./EzBus/EzBus.csproj" -c Release -p:PackageVersion=$version -o ./.artifacts
dotnet pack "./EzBus.RabbitMQ/EzBus.RabbitMQ.csproj" -c Release -p:PackageVersion=$version -o ./.artifacts
