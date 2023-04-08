#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 58080


FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["Echo-Plain-Text-Webserver/Echo-Plain-Text-Webserver.csproj", "Echo-Plain-Text-Webserver/"]
RUN dotnet restore "Echo-Plain-Text-Webserver/Echo-Plain-Text-Webserver.csproj"
COPY . .
WORKDIR "/src/Echo-Plain-Text-Webserver"
RUN dotnet build "Echo-Plain-Text-Webserver.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Echo-Plain-Text-Webserver.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Echo-Plain-Text-Webserver.dll"]
