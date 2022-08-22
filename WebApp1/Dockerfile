FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN mkdir /app
WORKDIR /app

COPY ./WebApp1/bin/Release/net6.0/publish ./
#COPY ./config.sh ./

#RUN bash config.sh ./

EXPOSE 80
ENTRYPOINT [ "dotnet", "WebApp1.dll" ]