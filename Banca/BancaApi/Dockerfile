#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443
#ENV StringaConn "Server= dbsql; Database= Banca; User= sa; Password=Password12345;"
FROM mcr.microsoft.com/dotnet/sdk:7.0  AS build
WORKDIR /src
COPY ["BancaApi/BancaApi.csproj", "BancaApi/"]
RUN dotnet restore "BancaApi/BancaApi.csproj"
COPY . .
WORKDIR "/src/BancaApi"
RUN dotnet build "BancaApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BancaApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BancaApi.dll"]

#FROM redis
#COPY redis.conf /usr/local/etc/redis/redis.conf
#CMD [ "redis-image", "/usr/local/etc/redis/redis.conf" ]