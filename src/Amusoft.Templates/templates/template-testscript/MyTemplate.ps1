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

# place your test script here
dotnet new template-test-script -n MyCustomTest

if($ExecuteUninstall) {
    if($IsWindows){
        dotnet new uninstall .\
    }
    else
    {
        dotnet new uninstall ./
    }
}