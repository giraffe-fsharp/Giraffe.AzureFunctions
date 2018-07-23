set -e
APP={APP}
USER={USER}
PASSWORD={PASSWORD}
GITURL=https://$USER:$PASSWORD@$APP.scm.azurewebsites.net:443/$APP.git
rm -rf build
dotnet publish -o build/Demo
pushd build
git init
git remote add origin $GITURL
git add -A
git commit -m "deploy"
git push $GITURL -f
popd