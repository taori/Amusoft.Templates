param (
    [Parameter(Mandatory=$false, HelpMessage="Execute installation of the template?")]
    [Boolean]$ExecuteInstall=$true,
    [Parameter(Mandatory=$false, HelpMessage="Execute uninstallation of the template?")]
    [Boolean]$ExecuteUninstall=$true,
    [Parameter(Mandatory=$false, HelpMessage="Use windows script paths?")]
    [Boolean]$IsWindows=$true
)

if($ExecuteInstall) {
    if($IsWindows){
        dotnet new install .\        
    }
    else
    {
        dotnet new install ./        
    }
}

$target = "D:\tmp\scaffolding6"

if(Test-Path $target){
    Remove-Item -Recurse -Path $target
}

New-Item -Type Directory -Path $target

try
{
    Push-Location $target

    # place your test script here
    dotnet new template-package -n TheFolderName `
--GitProjectName TheGitProject `
--NugetPackageId ThePackageId `
--ProductName TheProductName `
--GitUser TheGitUser `
--Author TheAuthor `
--RootBranchName main2 `

    explorer $target 
}
finally
{
    Pop-Location
}

if($ExecuteUninstall) {
    if($IsWindows){
        dotnet new uninstall .\
    }
    else
    {
        dotnet new uninstall ./
    }
}