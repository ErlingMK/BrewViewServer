$Yesterday = (get-date (get-date).addDays(-1) -Format "yyyy-MM-dd")
$Yesterday 

.\bin\Release\netcoreapp3.1\VinmonopolQuery.exe -mode $args -since $Yesterday 