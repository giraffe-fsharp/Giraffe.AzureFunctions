set -e
rm -rf build
dotnet publish demo -o build -c Release
cp *.json build
pushd build
func host start
popd