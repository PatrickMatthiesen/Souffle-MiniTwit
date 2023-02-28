# Souffle-MiniTwit

## Launching the Blazor project locally

1. (If not done before) Run the CreateDB script with Docker running.
    > `.\CreateDB.{ps1,sh}`
2. Run the Server Project
   - Use your IDE or
   - ```dotnet run --project .\MiniTwit\MiniTwit\Server```
3. Go to your browser and open `localhost:7089`

## Deploy to Docker containers

(This does not fully work yet and needs to be changed)

1. Make a trusted certificate and output it to a known location.
    > ```dotnet dev-certs https -ep <path to output?> -p <a strong password> --trust```
2. Add the password of the certificate to user-secrets
    > ```dotnet user-secrets set "Kestrel:Certificates:Development:Password" "$connectionString" --project <path to server>```
3. Publish the Docker image
    > ```dotnet publish .\MiniTwit\Server\ --os linux --arch x64 -c Release -p:PublishProfile=DefaultContainer --no-self-contained```
4. Run the container
    > ```dotnet run -it --rm minitwit-server:1.0.0```

## Deploying database

> This currently requires a person to do something, but would be updated to run from a script, how to do it using actions is unclear.

1. Make a droplet with docker support
2. SSH to the droplet and run
   `docker run --name MiniTwit-db -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<our strong password>" -p 1433:1433 -d mcr.microsoft.com/azure-sql-edge:latest`
3. Update the database with the dbcontext, by having the connection string to the database in user secrets and running
    `dotnet ef database update -p ./MiniTwit/Server/`
    > this should be changed.
