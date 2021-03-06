# Dotnet-XYZ © 2021

set -e
set -u

dotnet restore
dotnet build --configuration Release --no-restore /p:Version=${SERVICE_VERSION} /p:RepositoryUrl=${SERVICE_REPOSITORY_URL}
dotnet test --configuration Release --no-build --verbosity normal

dotnet nuget push **/*.nupkg --api-key ${NUGET_API_KEY} --source ${NUGET_SOURCE} --no-symbols true