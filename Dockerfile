FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SportsEcommerceAPI.csproj", "./"]
RUN dotnet restore "SportsEcommerceAPI.csproj"
COPY . .
RUN dotnet build "SportsEcommerceAPI.csproj" -c Release -o /src/build

FROM build AS publish
RUN dotnet publish "SportsEcommerceAPI.csproj" -c Release -o /src/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /src/publish .
ENTRYPOINT ["dotnet", "SportsEcommerceAPI.dll"]
