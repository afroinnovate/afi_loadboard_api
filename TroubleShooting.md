# Most common Errors and their Resolutions.

## Error

## Useful commands.

### docker-compose
1. ```docker-compose -f dev-docker-compose.yaml up``` start docker-compose iteractively
2. ```docker-compose -f dev-docker-compose.yaml down``` stop containers iteractively
3. ```docker ps``` To see all the runnning containers
4. ```docker rm -f $(docker ps -aq)``` Delete running and stopped containers.
5. ```docker ps -a``` list all the running and stopped containers
5. ```docker rm <container-id>```
6. ```docker rmi -f $(docker images -aq)``` remove all images even the one in use
7. ```docker image prun``` This command removes all dangling images, which are images that are not tagged and not associated with any containers.
8. ```docker container prune``` This command removes all stopped containers.
9. ```docker images``` List all the docker images
10. ```docker rmi <image_id_or_name>``` remove image by name
11. ```docker exec -it <container_id_or_name> /bin/bash``` Get into the container to check if it runs
