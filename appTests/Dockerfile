FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY appTests/appTests.csproj appTests/
COPY project_euler/app.csproj project_euler/
RUN dotnet restore appTests/appTests.csproj
COPY . .
WORKDIR /src/appTests

ENTRYPOINT ["dotnet", "test", "appTests.csproj"]
