FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
USER $APP_UID
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS publish
WORKDIR /src
COPY ["VehicleParking.Api/VehicleParking.Api.csproj", "VehicleParking.Api/"]
RUN dotnet restore "VehicleParking.Api/VehicleParking.Api.csproj"
COPY . .
RUN dotnet publish "VehicleParking.Api/VehicleParking.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VehicleParking.Api.dll"]
