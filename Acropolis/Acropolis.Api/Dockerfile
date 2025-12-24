FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
ARG user=DockerAppUser
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
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

# Switch to root to install system packages, then drop privileges back to non-root user.
USER root

ENV PUPPETEER_SKIP_CHROMIUM_DOWNLOAD=true

# Combine package installation into a single layer, use --no-install-recommends, then purge build-time tools and clean caches.
RUN set -eux; \
    apt-get update && \
    apt-get install -y --no-install-recommends \
        ca-certificates \
        ffmpeg \
        fonts-ipafont-gothic \
        fonts-wqy-zenhei \
        fonts-thai-tlwg \
        fonts-kacst \
        fonts-freefont-ttf \
        libxss1 \
        gnupg \
        dirmngr \
        wget \
        apt-transport-https && \
    # Add Google Chrome repo and install chrome
    wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - && \
    echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" > /etc/apt/sources.list.d/google.list && \
    apt-get update && \
    apt-get install -y --no-install-recommends google-chrome-stable && \
    # Remove packages only needed for the installation and clean apt caches to reduce image size
    apt-get purge -y --auto-remove wget gnupg dirmngr apt-transport-https && \
    rm -rf /var/lib/apt/lists/* /var/cache/apt/*

# Create user/group, app directories and set ownership in a single layer
RUN set -eux; \
    groupadd -r ${user} || true; \
    useradd -r -g ${user} ${user} || true; \
    mkdir -p /app/browser /app/data /home/${user} /home/${user}/Downloads; \
    chown -R ${user}:${user} /app /app/data /app/browser /home/${user}

USER ${user}

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Acropolis.Api.dll"]


# run from Acropolis: docker build . -t acropolis -f Acropolis.Api/Dockerfile
