# Dotnet-XYZ © 2021

set -e
set -u

# PostgreSQL

POSTGRES_VERSION=13.3

# Separate quiet pull for clean build logs.
docker pull --quiet ${POSTGRES_VERSION}

docker run \
	--name=postgres \
	-d --rm \
	-e "POSTGRES_PASSWORD=${POSTGRES_PASSWORD}" \
	--network=host \
	--log-driver=syslog \
	--log-opt tag=postgres \
	postgres:${POSTGRES_VERSION}

# docker build

# Separate quiet pull for clean build logs.
docker pull --quiet mcr.microsoft.com/dotnet/aspnet:5.0-alpine
docker pull --quiet mcr.microsoft.com/dotnet/sdk:5.0-alpine

docker build \
	--network=host \
	--build-arg NUGET_API_KEY=${NUGET_API_KEY} \
	--build-arg NUGET_SOURCE=${NUGET_SOURCE} \
	--build-arg SERVICE_REPOSITORY_URL=${SERVICE_REPOSITORY_URL} \
	--build-arg SERVICE_VERSION=${SERVICE_VERSION} \
	-t ${SERVICE_IMAGE_NAME}:${SERVICE_VERSION} \
	.
