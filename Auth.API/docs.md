# Auth API
this is the microservice that'll take care of all the authentication and authorization tasks.
It's implemented using .NET entity Framework and docker.

## Getting Started

2. **Navigate to the Project Directory**
    ```bash
    cd afi_loadboad_api/Services/Auth.API
    ```

3. **Restore the .NET Packages**
    ```bash
    dotnet restore
    ```

4. **Build the API docker image the API**
    
     #to override the default tag

    ```bash 
    dotnet publish --os linux --arch x64 --p:ContainerImageTage=0.0.1-alpha
    ```
    or 
       
    ```bash
      dotnet publish --os linux --arch x64
    ```
5. **Run the API docker image the API**
    ```bash
    docker run -p 5000:7000 auth-api:0.0.1-alpha
    ```

The API should now be running at `http://localhost:5000`.
You can visit the API documentation on `http://localhost:5000/swagger/index.html`

## Database Setup

This API uses PostgreSQL. To set it up:

1. **Install PostgreSQL** if not already installed.
   
3. **Update Connection String** in `appsettings.json` to point to your local PostgreSQL instance.

## Useful Docker Commands

---

### Containers

#### List Containers
- `docker ps`: List running containers
- `docker ps -a`: List all containers

#### Run Container
- `docker run <image>`: Run a container

#### Stop Container
- `docker stop <container_id/container_name>`: Stop a container

#### Start Container
- `docker start <container_id/container_name>`: Start a stopped container

#### Remove Container
- `docker rm <container_id/container_name>`: Remove a container

#### Logs
- `docker logs <container_id/container_name>`: View container logs

#### Execute Command Inside Container
- `docker exec -it <container_id/container_name> /bin/bash`: Execute a command inside a running container

#### Inspect Container
- `docker inspect <container_id/container_name>`: Inspect a container

#### Copy Files from/to Container
- `docker cp <container_id/container_name>:/path/to/file /local/path/to/file`: Copy files from or to a container

#### Kill Container
- `docker kill <container_id/container_name>`: Force stop a container

#### Restart Container
- `docker restart <container_id/container_name>`: Restart a container

---

### Images

#### List Images
- `docker images`: List all available images

#### Pull Image
- `docker pull <image>`: Download an image from a registry

#### Remove Image
- `docker rmi <image_id/image_name>`: Remove an image

#### Build Image
- `docker build -t <image_name> .`: Build an image from a Dockerfile

#### Tag Image
- `docker tag <source_image>:<tag> <target_image>:<tag>`: Tag an image

#### Push Image
- `docker push <image_name>`: Push an image to a registry

#### Inspect Image
- `docker inspect <image_id/image_name>`: Inspect an image

#### Save Image
- `docker save -o <path/to/save> <image_name>`: Save an image to a tar archive

#### Load Image
- `docker load -i <path/to/image/file>`: Load an image from a tar archive


