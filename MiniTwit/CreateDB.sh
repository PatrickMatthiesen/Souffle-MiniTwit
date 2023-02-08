Green='\033[0;32m'
NC='\033[0m' # No Color

echo -e "${Green}Starting SQL server in docker container...${NC}"
password="a282f131-5323-419c-9c8e-55535d486550" # New-Guid
docker rm --force MiniTwit-db
docker run --name MiniTwit-db -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/azure-sql-edge:latest
database="MiniTwit-db"
connectionString="Server=localhost;Database=$database;User Id=sa;Password=$password;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True"
echo ""

serverPath="./MiniTwit/Server/"

echo -e "${Green}Configuring User Secrets...${NC}"
echo "Configuring Connection String"
dotnet user-secrets set "ConnectionStrings:$database" "$connectionString" --project $serverPath
echo ""

echo -e "${Green}Trusting HTTPS development certificate...${NC}"
dotnet dev-certs https --trust
echo ""

echo -e "${Green}Updating database...${NC}"
dotnet ef database update -p $serverPath
echo ""