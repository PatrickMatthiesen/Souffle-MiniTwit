#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN dotnet dev-certs https --trust

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MiniTwit/Server/MiniTwit.Server.csproj", "MiniTwit/Server/"]
COPY ["MiniTwit/Client/MiniTwit.Client.csproj", "MiniTwit/Client/"]
COPY ["MiniTwit/Shared/MiniTwit.Shared.csproj", "MiniTwit/Shared/"]
RUN dotnet restore "MiniTwit/Server/MiniTwit.Server.csproj"
COPY . .
WORKDIR "/src/MiniTwit/Server"
RUN dotnet build "MiniTwit.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiniTwit.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniTwit.Server.dll"]