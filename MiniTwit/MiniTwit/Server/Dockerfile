#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MiniTwit/Server/MiniTwit.Server.csproj", "MiniTwit/Server/"]
COPY ["MiniTwit/Client/MiniTwit.Client.csproj", "MiniTwit/Client/"]
COPY ["MiniTwit/Shared/MiniTwit.Shared.csproj", "MiniTwit/Shared/"]
COPY ["MiniTwit/Infrastructure/Infrastructure.csproj", "MiniTwit/Infrastructure/"]
RUN dotnet restore "MiniTwit/Server/MiniTwit.Server.csproj"
COPY . .
WORKDIR "/src/MiniTwit/Server"
ENV ASPNETCORE_ENVIRONMENT Development
ENV MiniTwit-db "Server=MiniTwit-db;Database=MiniTwit-db;User Id=sa;Password=a282f131-5323-419c-9c8e-55535d486550;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True"

RUN dotnet build "MiniTwit.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiniTwit.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniTwit.Server.dll"]