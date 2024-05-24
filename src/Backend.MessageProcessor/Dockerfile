FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
 
COPY ["src/Backend.MessageProcessor/Backend.MessageProcessor.csproj", "src/Backend.MessageProcessor/"]
COPY ["Backend.Shared/Backend.Shared.csproj", "Backend.Shared/"]
COPY ["src/Backend.Communication.Base/Backend.Communication.Base.csproj", "src/Backend.Communication.Base/"]
RUN dotnet restore "./src/Backend.MessageProcessor/Backend.MessageProcessor.csproj"
 
COPY . .
WORKDIR "/src/src/Backend.MessageProcessor"
RUN dotnet build "./Backend.MessageProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/build
 
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Backend.MessageProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
 
ENTRYPOINT ["dotnet", "Backend.MessageProcessor.dll"]