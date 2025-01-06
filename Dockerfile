# Use the official .NET Core runtime image as a base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MagikarpMayhem.csproj", "./"]
RUN dotnet restore "MagikarpMayhem.csproj"

# Copy the rest of the source code and build it
COPY . .
WORKDIR "/src/"
RUN dotnet publish "MagikarpMayhem.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
# Copy the SQLite database file
COPY ["MagikarpMayhemContext.db", "./"]
ENTRYPOINT ["dotnet", "MagikarpMayhem.dll"]
