﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BackendAPI/BackendAPI.csproj", "BackendAPI/"]
RUN dotnet restore "BackendAPI/BackendAPI.csproj"
COPY . .
WORKDIR "/src/BackendAPI"
RUN dotnet build "BackendAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackendAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendAPI.dll"]
