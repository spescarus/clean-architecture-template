FROM postgres:latest as build

WORKDIR /app/sqitch

RUN apt-get update
RUN apt-get install sqitch dos2unix -y
RUN apt-get clean
COPY . .

RUN mkdir /app/bin
RUN cp -v *.sh /app/bin/
RUN dos2unix /app/bin/*.sh
RUN chmod +x /app/bin/*.sh
RUN chmod a+r /app/sqitch/*


ENTRYPOINT [ "/app/bin/entrypoint.sh" ]

EXPOSE 5432

CMD [ "postgres" ]