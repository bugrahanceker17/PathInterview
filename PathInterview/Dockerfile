FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
COPY /. ./

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["PathInterview/PathInterview.csproj", "PathInterview/"]
RUN dotnet restore "PathInterview/PathInterview.csproj"
COPY /. ./
RUN dotnet build "PathInterview.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PathInterview.csproj" -c Release -o /app/publish

#FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
#
#RUN apk add --no-cache icu-libs
#ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM base AS final
WORKDIR /app
COPY --from=base /app/publish .
ENTRYPOINT ["dotnet", "PathInterview.dll"]
