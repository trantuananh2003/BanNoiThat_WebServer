#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BanNoiThat.API/BanNoiThat.API.csproj", "BanNoiThat.API/"]
COPY ["BanNoiThat.Application/BanNoiThat.Application.csproj", "BanNoiThat.Application/"]
COPY ["BanNoiThat.Domain/BanNoiThat.Domain.csproj", "BanNoiThat.Domain/"]
COPY ["BanNoiThat.Infrastructure.SqlServer/BanNoiThat.Infrastructure.SqlServer.csproj", "BanNoiThat.Infrastructure.SqlServer/"]
RUN dotnet restore "./BanNoiThat.API/BanNoiThat.API.csproj"
COPY . .
WORKDIR "/src/BanNoiThat.API"
RUN dotnet build "./BanNoiThat.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BanNoiThat.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/certificates
COPY ["BanNoiThat.API/Certificate/aspnetapp.pfx", "/app/certificates/"]

ENTRYPOINT ["dotnet", "BanNoiThat.API.dll"]