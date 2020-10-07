$Today = Get-Date -Format "yyyy-MM-dd"
$Today

.\bin\Release\netcoreapp3.1\VinmonopolQuery.exe -mode $args -since $Today