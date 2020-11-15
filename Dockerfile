FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY *.sln .
COPY ["./Radicitus.Raffle/Radicitus.Raffle.csproj", "app/Radicitus.Raffle/"]
COPY ["./Radicitus.Data/Radicitus.Data.csproj", "app/Radicitus.Data/"]
COPY ["./Radicitus.Models/Radicitus.Models.csproj", "app/Radicitus.Models/"]
COPY ["./Radicitus.Redis/Radicitus.Redis.csproj", "app/Radicitus.Redis/"]
RUN dotnet restore "app/Radicitus.Raffle/Radicitus.Raffle.csproj"
COPY . .
RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -sL https://deb.nodesource.com/setup_15.x | bash -
RUN apt-get install -y nodejs
RUN npm rebuild node-sass --force
#RUN dotnet tool install --global dotnet-ef
#RUN export PATH="$PATH:/root/.dotnet/tools"
#RUN dotnet ef migrations script -o migrations.sql -p "/src/Radicitus.Health/Radicitus.Health.csproj"
RUN dotnet build "/src/Radicitus.Raffle/Radicitus.Raffle.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "/src/Radicitus.Raffle/Radicitus.Raffle.csproj" -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Radicitus.Raffle.dll"]