FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN dotnet restore "PathInterview/PathInterview.csproj"
COPY . .
RUN dotnet build "PathInterview.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PathInterview.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PathInterview.dll"]
