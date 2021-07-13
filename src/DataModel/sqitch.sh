#!/usr/bin/env bash

echo "########## Start Sqitch update ##########"
cd /app/sqitch/
sqitch status &>/dev/null
while [ $? -eq 2 ]
do
    echo "########## Database not ready yet ##########"
    sleep 1
    sqitch status &>/dev/null
done
sqitch deploy
echo "########## End Sqitch update ##########"
