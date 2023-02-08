Write-Host -ForegroundColor Green "Starting SQL server in docker container..."
$password = "a282f131-5323-419c-9c8e-55535d486550" # New-Guid
docker rm --force MiniTwit-db
docker run --name MiniTwit-db -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/azure-sql-edge:latest
$database = "MiniTwit-db"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True"
Write-Host ""

$serverPath = "./MiniTwit/Server/"

Write-Host -ForegroundColor Green "Configuring User Secrets..."
Write-Host "Configuring Connection String"
dotnet user-secrets set "ConnectionStrings:$database" "$connectionString" --project $serverPath
Write-Host ""

Write-Host -ForegroundColor Green "Trusting HTTPS development certificate..."

dotnet dev-certs https --trust
Write-Host ""

Write-Host -ForegroundColor Green "Updating database..."
dotnet ef database update -p $serverPath
Write-Host ""