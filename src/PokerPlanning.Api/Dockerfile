FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5011

ENV ASPNETCORE_URLS=http://+:5011

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["poker-planning-backend/src/PokerPlanning.Api/PokerPlanning.Api.csproj", "poker-planning-backend/src/PokerPlanning.Api/"]
RUN dotnet restore "poker-planning-backend/src/PokerPlanning.Api/PokerPlanning.Api.csproj"
COPY . .
WORKDIR "/src/poker-planning-backend/src/PokerPlanning.Api"
RUN dotnet build "PokerPlanning.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "PokerPlanning.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokerPlanning.Api.dll"]
