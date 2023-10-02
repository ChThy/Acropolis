FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG user=app
USER ${user}
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Acropolis.Api/Acropolis.Api.csproj", "Acropolis.Api/"]
RUN dotnet restore "Acropolis.Api/Acropolis.Api.csproj"
COPY . .
WORKDIR "/src/Acropolis.Api"
RUN dotnet build "Acropolis.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Acropolis.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Acropolis.Api.dll"]