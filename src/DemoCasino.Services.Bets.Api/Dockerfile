#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DemoCasino.Services.Bets.Api/DemoCasino.Services.Bets.Api.csproj", "src/DemoCasino.Services.Bets.Api/"]
COPY ["src/DemoCasino.Services.Bets.Application/DemoCasino.Services.Bets.Application.csproj", "src/DemoCasino.Services.Bets.Application/"]
COPY ["src/DemoCasino.Services.Bets.Core/DemoCasino.Services.Bets.Core.csproj", "src/DemoCasino.Services.Bets.Core/"]
COPY ["src/DemoCasino.Services.Shared/DemoCasino.Services.Shared.csproj", "src/DemoCasino.Services.Shared/"]
COPY ["src/DemoCasino.Services.Bets.Infrastructure/DemoCasino.Services.Bets.Infrastructure.csproj", "src/DemoCasino.Services.Bets.Infrastructure/"]
RUN dotnet restore "./src/DemoCasino.Services.Bets.Api/DemoCasino.Services.Bets.Api.csproj"
COPY . .
WORKDIR "/src/src/DemoCasino.Services.Bets.Api"
RUN dotnet build "./DemoCasino.Services.Bets.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DemoCasino.Services.Bets.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoCasino.Services.Bets.Api.dll"]