# Use the official image from the .NET SDK for building
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AzureBlobExplorerMVC/AzureBlobExplorerMVC.csproj", "AzureBlobExplorerMVC/"]
RUN dotnet restore "AzureBlobExplorerMVC/AzureBlobExplorerMVC.csproj"
COPY . .
WORKDIR "/src/AzureBlobExplorerMVC"
RUN dotnet build "AzureBlobExplorerMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AzureBlobExplorerMVC.csproj" -c Release -o /app/publish

# Copy the build artifacts to the final container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureBlobExplorerMVC.dll"]
