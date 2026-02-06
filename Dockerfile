FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY Directory.Build.props .
COPY src/RsGe.NET.Core/RsGe.NET.Core.csproj src/RsGe.NET.Core/
COPY src/RsGe.NET.WayBill/RsGe.NET.WayBill.csproj src/RsGe.NET.WayBill/
COPY src/RsGe.NET.Server/RsGe.NET.Server.csproj src/RsGe.NET.Server/
RUN dotnet restore src/RsGe.NET.Server/RsGe.NET.Server.csproj

COPY src/ src/
RUN dotnet publish src/RsGe.NET.Server/RsGe.NET.Server.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "RsGe.NET.Server.dll"]
