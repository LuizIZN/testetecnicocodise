# Development Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /app

COPY . .

RUN dotnet restore --project ./Api/TesteTecnico.csproj

EXPOSE 8080
ENTRYPOINT ["dotnet", "run", "--project", "./Api/TesteTecnico.csproj", "--no-launch-profile"]