FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
COPY domain src/domain
COPY data src/data
COPY api src/api
COPY tests src/tests
RUN dotnet test /src/tests
WORKDIR /src/api
RUN dotnet restore
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as development
RUN dotnet tool install dotnet-ef -g --version 7.0.13
ENV PATH="${PATH}:/root/.dotnet/tools"
COPY domain src/domain
COPY data src/data
COPY api src/api
WORKDIR /src/api
RUN dotnet restore
CMD dotnet run --no-launch-profile


FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine as final
COPY --from=build /src/api/out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "api.dll"]