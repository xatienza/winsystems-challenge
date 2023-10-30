FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app

COPY *.sln .
COPY src/WinSystems.Challenge.Core/*.csproj ./src/WinSystems.Challenge.Core/
COPY src/WinSystems.Challenge.Test/*.csproj ./src/WinSystems.Challenge.Test/
RUN dotnet restore

# copy full solution over
COPY . .
RUN dotnet build

FROM build AS testrunner
WORKDIR /app
ENTRYPOINT ["dotnet", "test", "--logger:trx"]
