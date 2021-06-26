# Dotnet-XYZ © 2021

# Build

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

ARG NUGET_API_KEY
ARG NUGET_SOURCE
ARG SERVICE_REPOSITORY_URL
ARG SERVICE_VERSION

COPY ./ /opt/service-x/
WORKDIR /opt/service-x/

RUN \
	NUGET_API_KEY=${NUGET_API_KEY} \
	NUGET_SOURCE=${NUGET_SOURCE} \
	SERVICE_REPOSITORY_URL=${SERVICE_REPOSITORY_URL} \
	SERVICE_VERSION=${SERVICE_VERSION} \
	sh -x /opt/service-x/build-dotnet.sh
RUN dotnet publish --configuration Release --no-build --output out ServiceX.Server

# Runtime

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine

COPY --from=build /opt/service-x/out/ /opt/service-x/
WORKDIR /opt/service-x/

ENTRYPOINT [ "dotnet", "DotnetXYZ.ServiceX.Server.dll" ]