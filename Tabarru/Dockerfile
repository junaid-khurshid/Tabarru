# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
# For more information, please see https://aka.ms/containercompat

# This stage is used when running from VS in fast mode (Default for Debug configuration)
# --- Base runtime image ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files for restore
COPY ["Tabarru/Tabarru.csproj", "Tabarru/"]
COPY ["Tabarru.Common/Tabarru.Common.csproj", "Tabarru.Common/"]
COPY ["Tabarru.Repositories/Tabarru.Repositories.csproj", "Tabarru.Repositories/"]
COPY ["Tabarru.Services/Tabarru.Services.csproj", "Tabarru.Services/"]

# Restore dependencies
RUN dotnet restore "Tabarru/Tabarru.csproj"

# Copy everything
COPY . .

WORKDIR "/src/Tabarru"
RUN dotnet build "Tabarru.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --- Publish stage ---
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tabarru.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --- Final runtime image ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tabarru.dll"]