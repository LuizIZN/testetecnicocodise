# Development Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /app

COPY . .

RUN dotnet restore ./Api/TesteTecnico.csproj

RUN dotnet build ./Api/TesteTecnico.csproj -c Release -o /app/build

EXPOSE 8080
ENTRYPOINT ["dotnet", "run", "--project", "./Api/TesteTecnico.csproj", "--no-launch-profile"]