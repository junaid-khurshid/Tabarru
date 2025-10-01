FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tabarru/Tabarru.csproj", "Tabarru/"]
COPY ["Tabarru.Common/Tabarru.Common.csproj", "Tabarru.Common/"]
COPY ["Tabarru.Repositories/Tabarru.Repositories.csproj", "Tabarru.Repositories/"]
COPY ["Tabarru.Services/Tabarru.Services.csproj", "Tabarru.Services/"]
RUN dotnet restore "./Tabarru/Tabarru.csproj"
COPY . .
WORKDIR "/src/Tabarru"
RUN dotnet build "./Tabarru.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tabarru.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tabarru.dll"]