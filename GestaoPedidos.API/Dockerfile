#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GestaoPedidos.API/GestaoPedidos.API.csproj", "GestaoPedidos.API/"]
COPY ["GestaoPedidos.Application/GestaoPedidos.Application.csproj", "GestaoPedidos.Application/"]
COPY ["GestaoPedidos.Domain/GestaoPedidos.Domain.csproj", "GestaoPedidos.Domain/"]
COPY ["GestaoPedidos.Infrastructure/GestaoPedidos.Infrastructure.csproj", "GestaoPedidos.Infrastructure/"]
RUN dotnet restore "./GestaoPedidos.API/GestaoPedidos.API.csproj"
COPY . .
WORKDIR "/src/GestaoPedidos.API"
RUN dotnet build "./GestaoPedidos.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GestaoPedidos.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copy source code for EF migrations
COPY --from=build /src .

ENTRYPOINT ["dotnet", "GestaoPedidos.API.dll"]
