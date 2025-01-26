#!/bin/bash

# Set these variables from GitHub Secrets
DOCKERHUB_USERNAME="${DOCKERHUB_USERNAME}"
DOCKERHUB_REPO="${DOCKERHUB_REPO}"
DOCKERHUB_ACCESS_TOKEN="${DOCKERHUB_ACCESS_TOKEN}"

# Calculate the date six months ago in ISO 8601 format
SIX_MONTHS_AGO=$(date -d "6 months ago" --iso-8601=seconds)

# Function to delete an image tag
delete_tag() {
    local tag=$1
    curl -s -X DELETE \
        -u "${DOCKERHUB_USERNAME}:${DOCKERHUB_ACCESS_TOKEN}" \
        "https://hub.docker.com/v2/repositories/${DOCKERHUB_USERNAME}/${DOCKERHUB_REPO}/tags/${tag}/"
    echo "Deleted tag: ${tag}"
}

# Get all tags in the repository
tags=$(curl -s -u "${DOCKERHUB_USERNAME}:${DOCKERHUB_ACCESS_TOKEN}" \
    "https://hub.docker.com/v2/repositories/${DOCKERHUB_USERNAME}/${DOCKERHUB_REPO}/tags/?page_size=100" | jq -r '.results[] | @base64')

# Loop through the tags and check their dates
for tag in ${tags}; do
    _jq() {
        echo "${tag}" | base64 --decode | jq -r "${1}"
    }

    tag_name=$(_jq '.name')
    tag_date=$(_jq '.last_updated')

    if [[ "${tag_date}" < "${SIX_MONTHS_AGO}" ]]; then
        delete_tag "${tag_name}"
    fi
done
