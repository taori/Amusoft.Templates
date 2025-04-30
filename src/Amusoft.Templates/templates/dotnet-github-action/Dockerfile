FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/MyConsoleApp/MyConsoleApp.csproj", "MyConsoleApp/"]
RUN dotnet restore MyConsoleApp/MyConsoleApp.csproj
COPY ["src/MyConsoleApp", "MyConsoleApp/"]
RUN dotnet publish MyConsoleApp/MyConsoleApp.csproj --configuration Release --no-restore --output /publish

# Label the container
LABEL maintainer="MAINTAINEREMAIL"
LABEL repository="https://github.com/GITOWNER/GITPROJECT"
LABEL homepage="https://github.com/GITOWNER/GITPROJECT"

# Label as GitHub Action
LABEL com.github.actions.name="TODO name"
LABEL com.github.actions.description="TODO description"
LABEL com.github.actions.icon="book-open"
LABEL com.github.actions.color="purple"

LABEL org.opencontainers.image.source=https://github.com/GITOWNER/GITPROJECT
LABEL org.opencontainers.image.description="TODO description"
LABEL org.opencontainers.image.licenses=MIT

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS final
WORKDIR /app
COPY --from=build /publish .
ENV DOTNET_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "/app/MyConsoleApp.dll"]