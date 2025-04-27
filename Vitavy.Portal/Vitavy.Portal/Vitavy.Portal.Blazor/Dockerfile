FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 5672

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Vitavy.Portal.Blazor/Vitavy.Portal.Blazor.csproj", "Vitavy.Portal.Blazor/"]
COPY ["Vitavy.Portal.Application/Vitavy.Portal.Application.csproj", "Vitavy.Portal.Application/"]
COPY ["Vitavy.Infrastructure.Abtraction/Vitavy.Infrastructure.Abtraction.csproj", "Vitavy.Infrastructure.Abtraction/"]
COPY ["Vitavy.Infrastructure/Vitavy.Infrastructure.csproj", "Vitavy.Infrastructure/"]
RUN dotnet restore "Vitavy.Portal.Blazor/Vitavy.Portal.Blazor.csproj"
COPY . .
WORKDIR "/src/Vitavy.Portal.Blazor"
RUN dotnet build "./Vitavy.Portal.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Vitavy.Portal.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vitavy.Portal.Blazor.dll"]
