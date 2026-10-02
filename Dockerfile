# ===== Stage 1: Build =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем csproj для кэша restore
COPY CinemaCatalog/src/CinemaCatalog.WebApi/CinemaCatalog.WebApi.csproj          CinemaCatalog/src/CinemaCatalog.WebApi/
COPY CinemaCatalog/src/CinemaCatalog.Application/CinemaCatalog.Application.csproj CinemaCatalog/src/CinemaCatalog.Application/
COPY CinemaCatalog/src/CinemaCatalog.Infrastructure/CinemaCatalog.Infrastructure.csproj CinemaCatalog/src/CinemaCatalog.Infrastructure/
COPY CinemaCatalog/src/CinemaCatalog.Domain/CinemaCatalog.Domain.csproj          CinemaCatalog/src/CinemaCatalog.Domain/
COPY CinemaCatalog/src/CinemaCatalog.Common/CinemaCatalog.Common.csproj          CinemaCatalog/src/CinemaCatalog.Common/

RUN dotnet restore CinemaCatalog/src/CinemaCatalog.WebApi/CinemaCatalog.WebApi.csproj

# Копируем весь исходный код backend
COPY CinemaCatalog/src/ CinemaCatalog/src/

WORKDIR /src/CinemaCatalog/src/CinemaCatalog.WebApi
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ===== Stage 2: Runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN mkdir -p /app/wwwroot/posters

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "CinemaCatalog.WebApi.dll"]
