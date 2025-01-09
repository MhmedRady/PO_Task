# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-noble AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-noble AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PO_Task.Api/PO_Task.Api.csproj", "PO_Task.Api/"]
COPY ["PO_Task.Application/PO_Task.Application.csproj", "PO_Task.Application/"]
COPY ["PO_Task.Domain/PO_Task.Domain.csproj", "PO_Task.Domain/"]
COPY ["PO_Task.Infrastructure/PO_Task.Infrastructure.csproj", "PO_Task.Infrastructure/"]
RUN dotnet restore "./PO_Task.Api/PO_Task.Api.csproj"
COPY . .
WORKDIR "/src/PO_Task.Api"
RUN dotnet build "./PO_Task.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PO_Task.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PO_Task.Api.dll"]