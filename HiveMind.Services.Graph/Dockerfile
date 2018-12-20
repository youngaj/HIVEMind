FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["HiveMind.Services.Graph/HiveMind.Services.Graph.csproj", "HiveMind.Services.Graph/"]
RUN dotnet restore "HiveMind.Services.Graph/HiveMind.Services.Graph.csproj"
COPY . .
WORKDIR "/src/HiveMind.Services.Graph"
RUN dotnet build "HiveMind.Services.Graph.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HiveMind.Services.Graph.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HiveMind.Services.Graph.dll"]