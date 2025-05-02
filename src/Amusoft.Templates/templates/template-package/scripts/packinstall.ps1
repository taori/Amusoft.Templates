Write-Host "Installing nupkg from `"$PSScriptRoot\..\artifacts\`"" -ForegroundColor Green
Get-ChildItem "$PSScriptRoot\..\artifacts\" -Filter "*.nupkg" | Foreach { dotnet new -i $_.FullName }