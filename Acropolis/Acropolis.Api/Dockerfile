FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG user=appuser
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

USER root

ENV PUPPETEER_SKIP_CHROMIUM_DOWNLOAD true
RUN apt-get update

RUN apt-get install -y wget gnupg \
    && wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - \
    && sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list' \
    && apt-get update \    
    && apt-get install -y google-chrome-stable fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf libxss1 --no-install-recommends \
    && rm -rf /var/lib/apt/lists/*

RUN groupadd -r ${user} && useradd -r -g ${user} ${user}
RUN mkdir -p /app/browser \
    && mkdir -p /app/data \
    && mkdir -p /home/${user} \
    && mkdir -p /home/${user}/Downloads \
    && chown -R ${user}:${user} /app/data \
    && chown -R ${user}:${user} /app \
    && chown -R ${user}:${user} /app/browser \
    && chown -R ${user}:${user} /home/${user}

USER ${user}

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Acropolis.Api.dll"]