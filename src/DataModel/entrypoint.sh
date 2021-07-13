#!/usr/bin/env bash

printf "[core]\n\tengine = pg\n[engine \"pg\"]\n\ttarget = db:pg://$POSTGRES_USER:$POSTGRES_PASSWORD@localhost:5432/$POSTGRES_DB\n" > /app/sqitch/sqitch.conf
printf "\\connect $POSTGRES_DB;\nCREATE EXTENSION pgcrypto;\n" > /docker-entrypoint-initdb.d/01-base-scripts.sql
sed "/docker_temp_server_stop$/i /app/bin/sqitch.sh" /usr/local/bin/docker-entrypoint.sh | sed "s/-c listen_addresses=''//" > /app/bin/docker-entrypoint.sh;
chmod +x /app/bin/docker-entrypoint.sh;
exec /app/bin/docker-entrypoint.sh $@