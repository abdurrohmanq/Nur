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
#
#COPY --from=build /app/publish .
#
#EXPOSE 80/tcp
#EXPOSE 443/tcp
#
#ENTRYPOINT ["dotnet", "Nur.WebAPI.dll"]

#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
#WORKDIR /App
#
#COPY src/backend/Nur.WebAPI/Nur.WebAPI.csproj ./src/backend/Nur.WebAPI/
#RUN dotnet restore ./src/backend/Nur.WebAPI/Nur.WebAPI.csproj
#
#COPY . .
#RUN dotnet publish -c Release -o out
#
## Build the runtime image
#FROM mcr.microsoft.com/dotnet/aspnet:8.0
#WORKDIR /App
#COPY --from=build-env /App/out .
#EXPOSE 5000
#ENV ASPNETCORE_URLS=http://+:5000
#ENTRYPOINT ["dotnet", "Nur.WebAPI.dll"]

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY ./Nur.WebAPI/Nur.WebAPI.csproj ./Nur.WebAPI/
RUN dotnet restore ./Nur.WebAPI/Nur.WebAPI.csproj
COPY . .
RUN dotnet publish -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 9050
ENV ASPNETCORE_URLS=http://+:9050
ENTRYPOINT ["dotnet", "Nur.WebAPI.dll"]
