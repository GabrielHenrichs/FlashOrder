FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FlashOrder.sln", "./"]
COPY ["src/FlashOrder.API/FlashOrder.API.csproj", "src/FlashOrder.API/"]
COPY ["src/FlashOrder.Application/FlashOrder.Application.csproj", "src/FlashOrder.Application/"]
COPY ["src/FlashOrder.Domain/FlashOrder.Domain.csproj", "src/FlashOrder.Domain/"]
COPY ["src/FlashOrder.Infra.Data/FlashOrder.Infra.Data.csproj", "src/FlashOrder.Infra.Data/"]
COPY ["tests/FlashOrder.Tests/FlashOrder.Tests.csproj", "tests/FlashOrder.Tests/"]

RUN dotnet restore "FlashOrder.sln"

COPY . .
RUN dotnet publish "src/FlashOrder.API/FlashOrder.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FlashOrder.API.dll"]
