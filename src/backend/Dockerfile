# ── Stage 1: Build ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia os arquivos de projeto primeiro para aproveitar o cache de layers do Docker
# (só re-executa o restore se os .csproj mudarem)
COPY WecWeatherTracker.slnx ./
COPY WecWeatherTracker.Domain/WecWeatherTracker.Domain.csproj             WecWeatherTracker.Domain/
COPY WecWeatherTracker.Application/WecWeatherTracker.Application.csproj   WecWeatherTracker.Application/
COPY WecWeatherTracker.Infrastructure/WecWeatherTracker.Infrastructure.csproj WecWeatherTracker.Infrastructure/
COPY WecWeatherTracker.Api/WecWeatherTracker.Api.csproj                   WecWeatherTracker.Api/

RUN dotnet restore WecWeatherTracker.slnx

# Copia o restante do código fonte e publica em modo Release
COPY . .
RUN dotnet publish WecWeatherTracker.Api/WecWeatherTracker.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ── Stage 2: Runtime ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copia apenas o artefato publicado (sem SDK — imagem final muito menor)
COPY --from=build /app/publish .

# Porta padrão do ASP.NET Core em containers
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "WecWeatherTracker.Api.dll"]
