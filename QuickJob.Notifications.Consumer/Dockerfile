FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["QuickJob.Notifications.Consumer.csproj", "./"]
RUN dotnet restore "QuickJob.Notifications.Consumer.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "QuickJob.Notifications.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuickJob.Notifications.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuickJob.Notifications.Consumer.dll"]
