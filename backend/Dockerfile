# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj és restore
COPY ["SocialMediaAPI/*.csproj", "SocialMediaAPI/"]
RUN dotnet restore "SocialMediaAPI/SocialMediaAPI.csproj"

# Copy minden file és build
COPY . .
WORKDIR "/src/SocialMediaAPI"
RUN dotnet build "SocialMediaAPI.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "SocialMediaAPI.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SocialMediaAPI.dll"]