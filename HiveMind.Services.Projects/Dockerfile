FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["HiveMind.Services.Projects/HiveMind.Services.Projects.csproj", "HiveMind.Services.Projects/"]
RUN dotnet restore "HiveMind.Services.Projects/HiveMind.Services.Projects.csproj"
COPY . .
WORKDIR "/src/HiveMind.Services.Projects"
RUN dotnet build "HiveMind.Services.Projects.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HiveMind.Services.Projects.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HiveMind.Services.Projects.dll"]