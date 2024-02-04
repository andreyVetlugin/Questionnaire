FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

ENV DOTNET_URLS=http://+:5000;http://+:5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Questionnaires.Web/Questionnaires.Web.csproj", "Questionnaires.Web/"]
RUN dotnet restore "Questionnaires.Web/Questionnaires.Web.csproj"
COPY . .
WORKDIR "/src/Questionnaires.Web"
RUN dotnet build "Questionnaires.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Questionnaires.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Questionnaires.Web.dll"]
