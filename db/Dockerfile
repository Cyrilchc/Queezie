FROM mysql:5.6

ENV MYSQL_ROOT_PASSWORD=queezie
ENV MYSQL_DATABASE=queezie

WORKDIR /app

# Éxécute le script au lancement
COPY queezie.sql /docker-entrypoint-initdb.d/

EXPOSE 3306 3306
CMD ["mysqld"]

