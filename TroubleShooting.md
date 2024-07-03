# Most common Errors and their Resolutions.

## Error
- AspNetRole already exists/
1. When running dotnet ef database update in the docker container to update the database and we get the permission issue.

2. System.ArgumentException: Host can't be null when running the container.
3.  Error response from daemon: failed to create task for container: failed to create shim task: OCI runtime create failed: runc create failed: unable to start container process: exec: "./docker-entrypoint.sh": permission denied: unknown.
ERRO[0002] error waiting for container: 

4. Exception data:
    Severity: ERROR
    SqlState: 42501
    MessageText: permission denied for table __EFMigrationsHistory
    File: aclchk.c
    Line: 3650
    Routine: aclcheck_error
42501: permission denied for table __EFMigrationsHistory

5.  Exception data:
          Severity: ERROR
          SqlState: 42501
          MessageText: permission denied for table AspNetUsers
          File: aclchk.c
          Line: 3650
          Routine: aclcheck_error

6. error sending email: Error occurred while sending email.: 535: 5.7.139 Authentication unsuccessful, the user credentials were incorrect. 

7. Exception data:
    Severity: ERROR
    SqlState: 42501
    MessageText: must be owner of table AspNetUsers
    File: aclchk.c
    Line: 3788
    Routine: aclcheck_error
42501: must be owner of table AspNetUsers

8. Unhandled exception. System.ArgumentException: To validate server certificates, please use VerifyFull or VerifyCA instead of Require. To disable validation, explicitly set 'Trust Server Certificate' to true. See https://www.npgsql.org/doc/release-notes/6.0.html for more details.

## Solution

Create a new Migration while having the second migration
- ```dotnet ef migrations add <secondMigration>```
Update the database with the new migration
```dotnet ef databse update secondMigration```

1. Make sure we grand all the privileges for that schema to our user (afroinnovate). which can be done using this command in the database query tool - GRANT ALL PRIVILEGES ON SCHEMA public TO afroinnovate;

2. The reason for failure number 2 is because we setup the launchSettings's ApplicationURLs is set to localhost:8080, while any image built not from the localhost will not run localhost:8080, it'll run from the host network interface - 0.0.0.0:8080, so the solution is to change the launchSettings to look like below. "applicationUrl": "https://0.0.0.0:443;http://0.0.0.0:8080"   "applicationUrl": "http://0.0.0.0:8080",

3. delete the already created image, then run  ```chmod +x ./docker-entrypoint.sh``` and rebuild the image, then run it.

4. Go to pgAdmin->loadboard->disconnect server->properties->connections->change user from afroinnovate to doadmin->save and close -> try to connect-you'd be prompted with password ->enter doadmin password.
    - Run these two commands
        1. ```GRANT USAGE ON SCHEMA public TO afroinnovate;```
        2. ```GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE "__EFMigrationsHistory" TO afroinnovate;```

5. Go to pgAdmin->loadboard->disconnect server->properties->connections->change user from afroinnovate to doadmin->save and close -> try to connect-you'd be prompted with password ->enter doadmin password.
    - Run the following commands
        1. ```GRANT USAGE ON SCHEMA public TO afroinnovate;```
        2. ```GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO your_db_user;``` Or
        2. ```ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO afroinnovate;```

6. following these steps:
    1. Sign into the google account you want to use for the sender
    2. Go to Security
    3. Type "app password" in the search bar
    4. provide appname and the password will be generated.
    5. copy that password into the appsettings.
7. Go to pgAdmin->loadboard->disconnect server->properties->connections->change user from afroinnovate to doadmin->save and close -> try to connect-you'd be prompted with password ->enter doadmin password.
    1. ALTER TABLE AspNetUsers OWNER TO afroinnovate;
8. Change the sslmode in the connection string. 
    1. If we want to enforce strict certificate validation (which is recommended for production environments), use sslmode=VerifyFull. This requires a valid certificate and a matching server name.
    2. Alternatively, if you want to require SSL but without server certificate validation (less secure, but sometimes used in controlled environments), use "sslmode=Require;Trust Server Certificate=true;"

9. File still shows as being tracked even if added to git ignore.
    1. git rm --cached <filename> e.g. appsettings.Development.json
## Useful commands.

### docker-compose
1. ```docker-compose -f dev-docker-compose.yaml up``` start docker-compose iteractively
2. ```docker-compose -f dev-docker-compose.yaml down``` stop containers iteractively
3. ```docker ps``` To see all the runnning containers
4. ```docker rm -f $(docker ps -aq)``` Delete running and stopped containers.
5. ```docker ps -a``` list all the running and stopped containers
6. ```docker rm <container-id>
7. ```docker rmi -f $(docker images -aq)``` remove all images even the one in use
8. ```docker image prun``` This command removes all dangling images, which are images that are not tagged and not      associated with any containers.
9. ```docker container prune``` This command removes all stopped containers.
10. ```docker images``` List all the docker images
11. ```docker rmi <image_id_or_name>``` remove image by name
12. ```docker exec -it <container_id_or_name> /bin/bash``` Get into the container to check if it runs
13. ```docker run -e "DefaultConnection=Host=loadboard-db-do-user-14760993-0.c.db.ondigitalocean.com;Port=25060;Database=Users;Username=username;Password=pw;sslMode=Prefer;" -p 8080:8080 api:v1```
14. ```docker exec -it <container-id> /bin/bash``` get into the docker container.
15. ```docker inspect --format '{{range .Config.Env}}{{println .}}{{end}}' container-id``` To view the environment variables in the container.
16. ```docker log <container-id>``` to see the logs in the container
