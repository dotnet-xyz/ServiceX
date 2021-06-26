# Dotnet-XYZ © 2021

set -e
set -u

# PostgreSQL

POSTGRES_VERSION=13.3

docker run \
	--name=postgres \
	--tty \
	-d --rm \
	-e "POSTGRES_PASSWORD=${POSTGRES_PASSWORD}" \
	--network=host \
	--log-driver=syslog \
	--log-opt tag=postgres \
	postgres:${POSTGRES_VERSION}

# docker build

docker build \
	--network=host \
	--build-arg NUGET_API_KEY=${NUGET_API_KEY} \
	--build-arg NUGET_SOURCE=${NUGET_SOURCE} \
	--build-arg SERVICE_REPOSITORY_URL=${SERVICE_REPOSITORY_URL} \
	--build-arg SERVICE_VERSION=${SERVICE_VERSION} \
	-t dotnet-xyz/service-x:${SERVICE_VERSION} \
	.
