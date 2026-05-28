FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG APP_VERSION=0.1.0
ARG IMAGE_TAG=local
ARG BUILD_DATE=
ARG GIT_SHA=local

COPY . .
RUN dotnet publish src/BAStudio.Chatbot.Api/BAStudio.Chatbot.Api.csproj -c Release -o /app/publish /p:Version=$APP_VERSION /p:InformationalVersion=$APP_VERSION+$GIT_SHA

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
ARG APP_VERSION=0.1.0
ARG IMAGE_TAG=local
ARG BUILD_DATE=
ARG GIT_SHA=local

COPY --from=build /app/publish .
COPY ChatBot/ba_manual_vector.db ./ChatBot/ba_manual_vector.db
COPY docs/product-manuals ./docs/product-manuals

ENV ASPNETCORE_URLS=http://+:8080
ENV CHATBOT_APP_VERSION=$APP_VERSION
ENV CHATBOT_IMAGE_TAG=$IMAGE_TAG
ENV CHATBOT_BUILD_DATE=$BUILD_DATE
ENV CHATBOT_GIT_SHA=$GIT_SHA
EXPOSE 8080

LABEL org.opencontainers.image.title="BAStudio Chatbot API"
LABEL org.opencontainers.image.version=$APP_VERSION
LABEL org.opencontainers.image.ref.name=$IMAGE_TAG
LABEL org.opencontainers.image.revision=$GIT_SHA
LABEL org.opencontainers.image.created=$BUILD_DATE

ENTRYPOINT ["dotnet", "BAStudio.Chatbot.Api.dll"]
