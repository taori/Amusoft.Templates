dotnet build ..\src\Amusoft.Templates.slnx -v q

Write-Host "Removing artifacts folder ..."
Remove-Item -Recurse -Force -Path ..\artifacts -ErrorAction SilentlyContinue
Write-Host "done." -ForegroundColor Green

Write-Host "Creating artifacts folder structure ..."
New-Item ..\artifacts -ItemType Directory | Out-Null
Write-Host "done." -ForegroundColor Green

dotnet pack ..\src\Amusoft.Templates\Amusoft.Templates.csproj -o ..\artifacts -v m