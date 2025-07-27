# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# В зависимости от операционной системы хост-компьютеров, которые будут выполнять сборку контейнеров или запускать их, может потребоваться изменить образ, указанный в инструкции FROM.
# Дополнительные сведения см. на странице https://aka.ms/containercompat

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["ProfitTest.API/ProfitTest.API.csproj", "ProfitTest.API/"]
COPY ["ProfitTest.Application/ProfitTest.Application.csproj", "ProfitTest.Application/"]
COPY ["ProfitTest.Contracts/ProfitTest.Contracts.csproj", "ProfitTest.Contracts/"]
COPY ["ProfitTest.Domain/ProfitTest.Domain.csproj", "ProfitTest.Domain/"]
COPY ["ProfitTest.Infrastructure/ProfitTest.Infrastructure.csproj", "ProfitTest.Infrastructure/"]
COPY ["ProfitTest.Persistence/ProfitTest.Persistence.csproj", "ProfitTest.Persistence/"]
RUN dotnet restore "ProfitTest.API/ProfitTest.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/ProfitTest.API"
RUN dotnet publish "ProfitTest.API.csproj" -c Release -o /app/publish

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProfitTest.API.dll"]