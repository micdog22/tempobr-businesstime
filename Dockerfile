
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/TempoBR.BusinessTime.Core/TempoBR.BusinessTime.Core.csproj src/TempoBR.BusinessTime.Core/
COPY src/TempoBR.BusinessTime.Api/TempoBR.BusinessTime.Api.csproj src/TempoBR.BusinessTime.Api/
COPY tests/TempoBR.BusinessTime.Tests/TempoBR.BusinessTime.Tests.csproj tests/TempoBR.BusinessTime.Tests/
RUN dotnet restore src/TempoBR.BusinessTime.Api/TempoBR.BusinessTime.Api.csproj
COPY . .
RUN dotnet build src/TempoBR.BusinessTime.Api/TempoBR.BusinessTime.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish src/TempoBR.BusinessTime.Api/TempoBR.BusinessTime.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "TempoBR.BusinessTime.Api.dll"]
