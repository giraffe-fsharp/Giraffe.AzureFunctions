#!/bin/sh
set -eu
cd "$(dirname "$0")"
dotnet tool restore
dotnet paket restore
dotnet publish demo -o build/demo -c Release
dotnet publish demo2 -o build/demo2 -c Release
dotnet test tests/Tests.fsproj -c Release
