$csproj = "$(Get-Location)/BrewView.Contracts.csproj"

$xml = New-Object XML
$xml.Load($csproj)
$xml.Project.PropertyGroup.Version = $args[0]
$xml.Save($csproj)

dotnet pack -o ./nugetpkg
dotnet nuget push "$(Get-Location)/nugetpkg/BrewView.Contracts.$($args[0]).nupkg" --api-key (Get-childitem env:NugetApiKey) --source https://api.nuget.org/v3/index.json