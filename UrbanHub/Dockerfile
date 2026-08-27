# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files relative to UrbanHub directory
COPY ["UrbanHub.web.csproj", "./"]
COPY ["../UrbanHub.Entities/UrbanHub.Entities.csproj", "../UrbanHub.Entities/"]
COPY ["../Urbanhub.Data/Urbanhub.Data.csproj", "../Urbanhub.Data/"]
COPY ["../UrbanHub.DTO/UrbanHub.DTO.csproj", "../UrbanHub.DTO/"]
COPY ["../UrbanHub.shared/UrbanHub.shared.csproj", "../UrbanHub.shared/"]
COPY ["../UrbanhubAuth.repo/UrbanHubManagement.repo.csproj", "../UrbanhubAuth.repo/"]

RUN dotnet restore "UrbanHub.web.csproj"

# Copy full source code
COPY . .
RUN dotnet publish "UrbanHub.web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "UrbanHub.web.dll"]