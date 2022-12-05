FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Property.Api/Property.Api.csproj", "Property.Api/"]
COPY ["src/Property.Api.Infrastructure/Property.Api.Infrastructure.csproj", "Property.Api.Infrastructure/"]
COPY ["src/Property.Api.Entities/Property.Api.Entities.csproj", "Property.Api.Entities/"]
COPY ["src/Property.Api.Contracts/Property.Api.Contracts.csproj", "Property.Api.Contracts/"]
COPY ["src/Property.Api.Core/Property.Api.Core.csproj", "Property.Api.Core/"]
RUN dotnet restore "src/Property.Api/Property.Api.csproj"
COPY . .
WORKDIR "/src/Property.Api"
RUN dotnet build "Property.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Property.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Property.Api.dll"]
