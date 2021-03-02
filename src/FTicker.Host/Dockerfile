FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/FTicker.Host/FTicker.Host.fsproj", "src/FTicker.Host/"]
RUN dotnet restore "src/FTicker.Host/FTicker.Host.fsproj"
COPY . .
WORKDIR "/src/src/FTicker.Host"
RUN dotnet build "FTicker.Host.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FTicker.Host.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FTicker.Host.dll"]