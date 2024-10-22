#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DemoCasino.Services.Funds.Api/DemoCasino.Services.Funds.Api.csproj", "src/DemoCasino.Services.Funds.Api/"]
COPY ["src/DemoCasino.Services.Funds.Application/DemoCasino.Services.Funds.Application.csproj", "src/DemoCasino.Services.Funds.Application/"]
COPY ["src/DemoCasino.Services.Funds.Core/DemoCasino.Services.Funds.Core.csproj", "src/DemoCasino.Services.Funds.Core/"]
COPY ["src/DemoCasino.Services.Shared/DemoCasino.Services.Shared.csproj", "src/DemoCasino.Services.Shared/"]
COPY ["src/DemoCasino.Services.Funds.Infrastructure/DemoCasino.Services.Funds.Infrastructure.csproj", "src/DemoCasino.Services.Funds.Infrastructure/"]
RUN dotnet restore "./src/DemoCasino.Services.Funds.Api/DemoCasino.Services.Funds.Api.csproj"
COPY . .
WORKDIR "/src/src/DemoCasino.Services.Funds.Api"
RUN dotnet build "./DemoCasino.Services.Funds.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DemoCasino.Services.Funds.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoCasino.Services.Funds.Api.dll"]