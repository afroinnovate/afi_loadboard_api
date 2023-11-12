#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7070
EXPOSE 4431

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Frieght.Api.csproj", "."]
RUN dotnet restore "Frieght.Api.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "Frieght.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Frieght.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frieght.Api.dll"]