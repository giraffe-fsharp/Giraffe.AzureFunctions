set -e
rm -rf build
if [ ! -d .paket ]; then
  dotnet tool install paket --tool-path .paket
  .paket/paket install
fi
dotnet publish demo -o ../build -c Release /p:DebugSymbols=false /p:DebugType=None /p:AssemblyVersion=$ASSEMBLYVERSION