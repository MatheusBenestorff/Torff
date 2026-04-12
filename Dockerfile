#Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY . .

WORKDIR /app/src
RUN dotnet publish Torff.csproj -c Release -o /out

#Run
FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app

COPY --from=build /out .
COPY src/wwwroot ./wwwroot

EXPOSE 8080

ENTRYPOINT ["dotnet", "Torff.dll"]