pushd ..\..

powershell -NoProfile -ExecutionPolicy Bypass -Command ".\.files\Packaging\BuildNupkg.ps1"

popd