#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /app
#
##Copy resources to docket layer
#COPY . .
#
##Build artifact
#
#RUN cd ./src/backend/Nur.WebAPI && \
    #dotnet restore && \
    #dotnet build "Nur.WebAPI.csproj" -c Release --no-restore && \
    #dotnet publish "Nur.WebAPI.csproj" -c Release --no-build -o /app/publish
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS app
#WORKDIR /app
# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY ./Nur.Bot/Nur.Bot.csproj ./Nur.Bot/
RUN dotnet restore ./Nur.Bot/Nur.Bot.csproj
COPY . .
RUN dotnet publish -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 9060
ENV ASPNETCORE_URLS=http://+:9060
ENTRYPOINT ["dotnet", "Nur.Bot.dll"]
