$configuration = "Release"
$verbosity = "m"
$versionSuffix = "beta"
$runTest = $false
$runPack = $true

$dateFormat = [System.DateTime]::Now.ToString("yyyyMMdd.HHmm")
$versionSuffix = "$($versionSuffix).$($dateFormat)"

dotnet restore "$PSScriptRoot/../src/All.sln" --verbosity $verbosity
Write-Host "Restore complete" -ForegroundColor Green

dotnet build "$PSScriptRoot/../src/All.sln" --verbosity $verbosity -c $configuration --no-restore
Write-Host "Build complete" -ForegroundColor Green

if($runTest){
  dotnet test "$PSScriptRoot/../src/All.sln" --verbosity $verbosity -c $configuration --no-build 
  Write-Host "Test complete" -ForegroundColor Green
}

if($runPack){
  dotnet pack "$PSScriptRoot/../src/MyLibrary/MyLibrary.csproj" --verbosity $verbosity -c $configuration -o ../artifacts/nupkg --no-build /p:VersionSuffix=$versionSuffix
}
