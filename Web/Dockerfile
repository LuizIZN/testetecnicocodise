# Development Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /app

COPY . .

RUN dotnet restore ./Web/TesteTecnico.Web.csproj

RUN dotnet build ./Web/TesteTecnico.Web.csproj -c Release -o /app/build

EXPOSE 80
WORKDIR /app/build
ENTRYPOINT ["dotnet", "TesteTecnico.Web.dll"]