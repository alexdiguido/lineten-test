#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LineTenTest.Api/LineTenTest.Api.csproj", "LineTenTest.Api/"]
COPY ["LineTenTest.Domain/LineTenTest.Domain.csproj", "LineTenTest.Domain/"]
COPY ["LineTenTest.SharedKernel/LineTenTest.SharedKernel.csproj", "LineTenTest.SharedKernel/"]
COPY ["LineTenTest.Infrastructure/LineTenTest.Infrastructure.csproj", "LineTenTest.Infrastructure/"]
RUN dotnet restore "LineTenTest.Api/LineTenTest.Api.csproj"
COPY . .
WORKDIR "/src/LineTenTest.Api"
RUN dotnet build "LineTenTest.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LineTenTest.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LineTenTest.Api.dll"]