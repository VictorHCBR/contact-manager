FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ContactManager.sln ./
COPY Directory.Build.props ./
COPY src/ContactManager.Api/ContactManager.Api.csproj src/ContactManager.Api/
COPY src/ContactManager.Application/ContactManager.Application.csproj src/ContactManager.Application/
COPY src/ContactManager.Domain/ContactManager.Domain.csproj src/ContactManager.Domain/
COPY src/ContactManager.Infrastructure/ContactManager.Infrastructure.csproj src/ContactManager.Infrastructure/
COPY tests/ContactManager.UnitTests/ContactManager.UnitTests.csproj tests/ContactManager.UnitTests/

RUN dotnet restore src/ContactManager.Api/ContactManager.Api.csproj

COPY . .
RUN dotnet publish src/ContactManager.Api/ContactManager.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Docker
EXPOSE 8080
ENTRYPOINT ["dotnet", "ContactManager.Api.dll"]
