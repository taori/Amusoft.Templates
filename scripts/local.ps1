param(
    [ValidateSet("Install", "Uninstall","Update")]
    $Action = "Install"
)

$r = $PSScriptRoot
$sourceFolder = Resolve-Path "$r/../src/Amusoft.Templates/templates"

switch ($Action)
{
    "Install" {
        Get-ChildItem -Path $sourceFolder `
            | Select-Object -ExpandProperty FullName `
            | %{ &dotnet new install $_ }
    }
    "Uninstall" {
        Get-ChildItem -Path $sourceFolder `
            | Select-Object -ExpandProperty FullName `
            | %{ &dotnet new uninstall $_ }        
    }
    "Update" {
        Get-ChildItem -Path $sourceFolder `
            | Select-Object -ExpandProperty FullName `
            | %{ &dotnet new uninstall $_ }
        Get-ChildItem -Path $sourceFolder `
            | Select-Object -ExpandProperty FullName `
            | %{ &dotnet new install $_ }    
    }
    Default {
        Write-Error "Unknown Action type"
    }
}