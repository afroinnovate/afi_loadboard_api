#!/bin/bash

# Set the name or ID of the container you want to keep running
CONTAINER_TO_KEEP=""
# Find the image used by the container you want to keep
IMAGE_TO_KEEP=$(docker inspect --format='{{.Config.Image}}' "$CONTAINER_TO_KEEP")

echo "Stopping all running containers except '${CONTAINER_TO_KEEP}'..."
docker ps -q | grep -v $(docker ps -q --filter name="$CONTAINER_TO_KEEP") | xargs -r docker stop

echo "Removing all stopped containers..."
docker container prune -f

echo "Removing all unused images except '${IMAGE_TO_KEEP}'..."
docker images -q | grep -v $(docker images --filter=reference="$IMAGE_TO_KEEP" -q) | xargs -r docker rmi

echo "Removing all unused networks..."
docker network prune -f

echo "Cleanup completed."