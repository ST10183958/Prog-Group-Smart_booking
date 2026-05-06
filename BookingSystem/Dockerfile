FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install certs (fixes HTTPS/NuGet issues)
RUN apt-get update && apt-get install -y ca-certificates

# Copy project files FIRST (better restore reliability)
COPY . .

# Restore with verbose output (helps debugging)
RUN dotnet restore BookingSystem.csproj

# Publish
RUN dotnet publish BookingSystem.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BookingSystem.dll"]