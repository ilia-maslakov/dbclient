#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["dbclient/dbclient.csproj", "dbclient/"]
RUN dotnet restore "dbclient/dbclient.csproj"
COPY . .
WORKDIR "/src/dbclient"
RUN dotnet build "dbclient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dbclient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dbclient.dll"]