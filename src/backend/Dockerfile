# ── Stage 1: Build ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY . .
RUN dotnet restore WecWeatherTracker.Api/WecWeatherTracker.Api.csproj
RUN dotnet publish WecWeatherTracker.Api/WecWeatherTracker.Api.csproj -c Release -o /app/publish

# ── Stage 2: Runtime ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app

# Copia apenas o artefato publicado (sem SDK — imagem final muito menor)
COPY --from=build /app/publish .

# Porta padrão do ASP.NET Core em containers
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "WecWeatherTracker.Api.dll"]
