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

$target = "D:\tmp\scaffolding5"

if(Test-Path $target){
    Remove-Item -Recurse -Path $target
}

# place your test script here
dotnet new dotnet-template -o $target -n MyCustomTest -au Amusoft

explorer $target

if($ExecuteUninstall) {
    if($IsWindows){
        dotnet new uninstall .\
    }
    else
    {
        dotnet new uninstall ./
    }
}