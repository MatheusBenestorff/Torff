#Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish src/Torff.Server/Torff.Server.csproj -c Release -o /out

#Run
FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app

COPY --from=build /out .
COPY src/Torff.Server/wwwroot ./wwwroot
COPY torff.json .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Torff.Server.dll"]