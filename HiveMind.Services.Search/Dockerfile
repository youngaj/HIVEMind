FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["HiveMind.Services.Search/HiveMind.Services.Search.csproj", "HiveMind.Services.Search/"]
RUN dotnet restore "HiveMind.Services.Search/HiveMind.Services.Search.csproj"
COPY . .
WORKDIR "/src/HiveMind.Services.Search"
RUN dotnet build "HiveMind.Services.Search.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HiveMind.Services.Search.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HiveMind.Services.Search.dll"]