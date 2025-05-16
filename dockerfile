FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish ./BanNoiThat.API/BanNoiThat.API.csproj -c Release -o /app/

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
ENV DOTNET_URLS=http://+:5000
COPY --from=build /app .
ENTRYPOINT ["dotnet", "BanNoiThat.API.dll"]