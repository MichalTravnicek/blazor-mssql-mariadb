FROM mcr.microsoft.com/mssql/server:2019-latest
   
COPY --chmod=444 ./initdb.sql /
COPY --chmod=554 ./init.sh /
ENTRYPOINT /init.sh & /opt/mssql/bin/sqlservr
