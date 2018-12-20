FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["HiveMind.Services.Problems/HiveMind.Services.Problems.csproj", "HiveMind.Services.Problems/"]
RUN dotnet restore "HiveMind.Services.Problems/HiveMind.Services.Problems.csproj"
COPY . .
WORKDIR "/src/HiveMind.Services.Problems"
RUN dotnet build "HiveMind.Services.Problems.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HiveMind.Services.Problems.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HiveMind.Services.Problems.dll"]