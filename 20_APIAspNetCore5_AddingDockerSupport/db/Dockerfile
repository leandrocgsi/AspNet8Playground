FROM mysql:5.7.22
EXPOSE 3306
COPY ./APIAspNetCore5/db/migrations/ /home/database/
COPY ./APIAspNetCore5/db/dataset/ /home/database/
COPY ./APIAspNetCore5/ci/init_database.sh /docker-entrypoint-initdb.d/init_database.sh