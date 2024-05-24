FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app 
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Backend.Presentation.API/Backend.Presentation.API.csproj", "src/Backend.Presentation.API/"]
COPY ["Backend.Shared/Backend.Shared.csproj", "Backend.Shared/"]
COPY ["src/Backend.Application/Backend.Application.csproj", "src/Backend.Application/"]
COPY ["src/Backend.Communication.Base/Backend.Communication.Base.csproj", "src/Backend.Communication.Base/"]
COPY ["src/Backend.Domain/Backend.Domain.csproj", "src/Backend.Domain/"]
COPY ["src/Backend.Infrastructure/Backend.Infrastructure.csproj", "src/Backend.Infrastructure/"]
RUN dotnet restore "./src/Backend.Presentation.API/Backend.Presentation.API.csproj"
COPY . .
WORKDIR "/src/src/Backend.Presentation.API"
RUN dotnet build "./Backend.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Backend.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.Presentation.API.dll"]