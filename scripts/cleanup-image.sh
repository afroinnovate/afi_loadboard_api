#!/bin/bash

# Remove dangling images (images that are not tagged and not used by any container)
echo "Removing dangling Docker images..."
docker image prune -af

echo "Dangling images cleanup completed."