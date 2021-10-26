FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim-amd64 AS publish
ARG RELEASE_VERSION
WORKDIR /src
COPY ["Trove/Trove.Api.csproj", "Trove/"]
RUN dotnet restore "Trove/Trove.Api.csproj"
COPY . .
WORKDIR "/src/Trove"
RUN dotnet publish "Trove.Api.csproj" -c Release -p:VersionPrefix=$RELEASE_VERSION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trove.Api.dll"]