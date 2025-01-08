FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MagikarpMayhem.csproj", "./"]
RUN dotnet restore "MagikarpMayhem.csproj"

COPY . .
WORKDIR "/src/"
RUN dotnet publish "MagikarpMayhem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY ["MagikarpMayhemContext.db", "./"]
ENTRYPOINT ["dotnet", "MagikarpMayhem.dll"]
