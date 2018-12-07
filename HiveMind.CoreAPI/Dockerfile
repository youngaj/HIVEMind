FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["HiveMind.CoreAPI/HiveMind.CoreAPI.csproj", "HiveMind.CoreAPI/"]
RUN dotnet restore "HiveMind.CoreAPI/HiveMind.CoreAPI.csproj"
COPY . .
WORKDIR "/src/HiveMind.CoreAPI"
RUN dotnet build "HiveMind.CoreAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HiveMind.CoreAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HiveMind.CoreAPI.dll"]