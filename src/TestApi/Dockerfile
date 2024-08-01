FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
RUN apt-get update && apt-get install -y curl
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "TestApi.dll"]