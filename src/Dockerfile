FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5011

COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

ENV ASPNETCORE_URLS=http://+:5011

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /source
COPY . .
RUN dotnet restore
RUN dotnet build -c $configuration -o /app/build

ENV DOTNET_ROOT=/usr/share/dotnet
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef --version 7.0.14
RUN dotnet ef migrations bundle \
    --project PokerPlanning.Infrastructure/PokerPlanning.Infrastructure.csproj \
    --startup-project PokerPlanning.Api/PokerPlanning.Api.csproj \
    --output /app/efbundle \
    --self-contained -r linux-x64

FROM build AS publish
ARG configuration=Release
RUN dotnet publish -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

COPY --from=publish /app/publish .
COPY --from=build /app/efbundle .

ENTRYPOINT ["/bin/sh", "/app/entrypoint.sh"]
