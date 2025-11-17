FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Formster.Presentation/Formster.Presentation.csproj", "Formster.Presentation/"]
COPY ["Formster.Application/Formster.Application.csproj", "Formster.Application/"]
COPY ["Formster.Domain/Formster.Domain.csproj", "Formster.Domain/"]
COPY ["Formster.Infrastructure/Formster.Infrastructure.csproj", "Formster.Infrastructure/"]
RUN dotnet restore "Formster.Presentation/Formster.Presentation.csproj"
COPY . .
WORKDIR "/src/Formster.Presentation"
RUN dotnet build "./Formster.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Formster.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Formster.Presentation.dll"]
