# Development Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /app

COPY . .

RUN dotnet restore ./TesteTecnico.Web.csproj

RUN dotnet build ./TesteTecnico.Web.csproj -c Release -o /app/build

EXPOSE 80
WORKDIR /app/Web
ENTRYPOINT ["dotnet", "run", "--project", "TesteTecnico.Web.csproj", "--no-launch-profile"]