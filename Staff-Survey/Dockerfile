FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Staff-Survey/Staff-Survey.csproj", "Staff-Survey/"]
RUN dotnet restore "Staff-Survey/Staff-Survey.csproj"
COPY . .
WORKDIR "/src/Staff-Survey"
RUN dotnet build "Staff-Survey.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Staff-Survey.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Staff-Survey.dll"]