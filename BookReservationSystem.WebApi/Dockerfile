# Use the ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the .NET SDK on Alpine for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files to the container
COPY ["BookReservationSystem.WebApi/BookReservationSystem.WebApi.csproj", "BookReservationSystem.WebApi/"]
COPY ["BookReservationSystem.Application/BookReservationSystem.Application.csproj", "BookReservationSystem.Application/"]
COPY ["BookReservationSystem.Infrastructure/BookReservationSystem.Infrastructure.csproj", "BookReservationSystem.Infrastructure/"]
COPY ["BookReservationSystem.Domain/BookReservationSystem.Domain.csproj", "BookReservationSystem.Domain/"]

# Copy appsettings.json
COPY ["BookReservationSystem.WebApi/appsettings.json", "BookReservationSystem.WebApi/"]

# Restore dependencies
RUN dotnet restore "BookReservationSystem.WebApi/BookReservationSystem.WebApi.csproj"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/BookReservationSystem.WebApi"
RUN dotnet build "BookReservationSystem.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "BookReservationSystem.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookReservationSystem.WebApi.dll"]
