# Multi-stage build for .NET 8 ASP.NET Core app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy everything and restore/publish the specific project
COPY . .
RUN dotnet restore "src/WeekDayWebApplication/WeekDayWebApplication/WeekDayWebApplication.csproj"
RUN dotnet publish "src/WeekDayWebApplication/WeekDayWebApplication/WeekDayWebApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
# + means binding to all network interfaces not just localhost
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "WeekDayWebApplication.dll"]