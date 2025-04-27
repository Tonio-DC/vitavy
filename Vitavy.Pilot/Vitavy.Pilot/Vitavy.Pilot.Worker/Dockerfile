FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Vitavy.Pilot.Worker/Vitavy.Pilot.Worker.csproj", "Vitavy.Pilot.Worker/"]
RUN dotnet restore "Vitavy.Pilot.Worker/Vitavy.Pilot.Worker.csproj"
COPY . .
WORKDIR "/src/Vitavy.Pilot.Worker"
RUN dotnet build "./Vitavy.Pilot.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Vitavy.Pilot.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vitavy.Pilot.Worker.dll"]
