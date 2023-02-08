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
