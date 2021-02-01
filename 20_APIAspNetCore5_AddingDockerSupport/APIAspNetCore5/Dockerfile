#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["APIAspNetCore5/APIAspNetCore5.csproj", "APIAspNetCore5/"]
RUN dotnet restore "APIAspNetCore5/APIAspNetCore5.csproj"
COPY . .
WORKDIR "/src/APIAspNetCore5"
RUN dotnet build "APIAspNetCore5.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "APIAspNetCore5.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIAspNetCore5.dll"]
