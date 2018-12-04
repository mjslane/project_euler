FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY project_euler/app.csproj project_euler/
RUN dotnet restore project_euler/app.csproj
COPY . .
WORKDIR /src/project_euler
RUN dotnet build app.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish app.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "app.dll"]
