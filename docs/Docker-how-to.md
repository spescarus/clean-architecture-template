<p align="center">
  <a href="https://www.docker.com/">
    <img src="https://www.docker.com/sites/default/files/horizontal_large.png" height=160/>
  </a>

  <h3 align="center">Docker Guide</h3>
</p>


# Table of content

- [Table of content](#table-of-content)
- [What is Docker](#what-is-docker)
- [Definition](#definition)
  - [Dockerfile](#dockerfile)
  - [Image](#image)
  - [Container](#container)
  - [Volume](#volume)
  - [Compose](#compose)
- [Why using Docker in Development context ?](#why-using-docker-in-development-context-)
- [How to use docker with our repositories ?](#how-to-use-docker-with-our-repositories-)
  - [Does my repository use Docker ?](#does-my-repository-use-docker-)
  - [How to use docker-compose ?](#how-to-use-docker-compose-)
    - [Compose pull](#compose-pull)
    - [compose build](#compose-build)
    - [compose up](#compose-up)
    - [compose down](#compose-down)
- [Cheatsheet](#cheatsheet)
  - [I want to run the dependencies only](#i-want-to-run-the-dependencies-only)
  - [I want to run everything](#i-want-to-run-everything)
  - [I want to clean everything](#i-want-to-clean-everything)
# What is Docker

> Docker is a set of platform as a service products that uses OS-level virtualization to deliver software in packages called containers. Containers are isolated from one another and bundle their own software, libraries and configuration files; they can communicate with each other through well-defined channels

Source: [Wikipedia](https://en.wikipedia.org/wiki/Docker_(software))

# Definition

## Dockerfile

> A Dockerfile is a text document that contains all the commands you would normally execute manually in order to build a Docker [image](#image). Docker can build images automatically by reading the instructions from a Dockerfile.

Source: [Glossary][1]


## Image 

> Docker images are the basis of [containers](#container). An Image is an ordered collection of root filesystem changes and the corresponding execution parameters for use within a container runtime. An image typically contains a union of layered filesystems stacked on top of each other. An image does not have state and it never changes.

Source: [Glossary][1]

## Container

> A container is a runtime instance of a docker [image](#image).
> 
> A Docker container consists of
> - A Docker image
> - An execution environment
> - A standard set of instructions
> 
> The concept is borrowed from Shipping Containers, which define a standard to ship goods globally. Docker defines a standard to ship software.

Source: [Glossary][1]

## Volume

>A volume is a specially-designated directory within one or more containers that bypasses the Union File System. Volumes are designed to persist data, independent of the container’s life cycle. Docker therefore never automatically deletes volumes when you remove a container, nor will it “garbage collect” volumes that are no longer referenced by a container. Also known as: data volume
>
>There are three types of volumes: host, anonymous, and named:
>
>A host volume lives on the Docker host’s filesystem and can be accessed from within the container.
>
>A named volume is a volume which Docker manages where on disk the volume is created, but it is given a name.
>
>An anonymous volume is similar to a named volume, however, it can be difficult, to refer to the same volume over time when it is an anonymous volumes. Docker handle where the files are stored.

Source: [Glossary][1]
## Compose

> A command-line tool and YAML file format with metadata for defining and running multi-container applications. You define a single application based on multiple images with one or more .yml files that can override values depending on the environment. After you've created the definitions, you can deploy the whole multi-container application with a single command (docker-compose up) that creates a container per image on the Docker host.

Source : [Glossary][2]

# Why using Docker in Development context ?
We use Docker in development to run complex preconfigured services package on your laptop. Instead of running each service one by one and try to configure everything. Docker provides a ready-to-use preconfigured environment.

# How to use docker with our repositories ?

## Does my repository use Docker ?

If you find at the root of your repository a file named ***docker-compose.yml***, so yes !

## How to use docker-compose ?

Docker-compose will run for you the current project and every dependencies needed. This include, database and other exotic dependencies (Memory cache and so on...).

You need to know some base command to be an docker expert !

First of all, the command on Linux, Mac OSX and Windows with Powershell 7+ are the same. If you are on Windows with **Command Line** or **Basic Powershell**, you need to replace every **&&** by **;**.

### Compose pull

> Pulls an image associated with a service defined in a docker-compose.yml or docker-stack.yml file, but does not start containers based on those images. [More...][3]

So, if you run docker-compose pull, you will download the latest image from the repository.

This command should be run after *git pull* on develop branch. *git pull* will update the source code and *docker-compose pull* will update the local images define in the yaml file.

**Sample:**

On Template WebApi, the first dependencies available is the libraries micro service. docker-compose pull recovered the latest image of the libraries micro service and his database.
```
❯ docker-compose pull
Pulling webapi_database           ... done
Pulling webapi                    ... done
WARNING: Some service image(s) must be built from source by running:
    docker-compose build webapi_database webapi
```

The command line warn use than some of image need to be build. Let's go to [build some images](#compose-build) !

### compose build

Build the image associate with a service defined in a docker-compose.yml, but does not start containers based on those images. [More...][4]

You need to run this command if there are any change in the source code of the repository. it will create new images of the project to be update to date with the code.

### compose up

> Builds, (re)creates, starts, and attaches to containers for a service. [More...][5]

This command will start the images built and the downloaded dependencies and attach it to the current terminal. By default, the command run every services specify into the docker-compose.yml file.

You can specify what service you want to run by giving the name of the service after the *up*.

Sample:

On Template WebApi:

Run everything
```
> docker-compose up
```

Run only the micro service
```
> docker-compose up webapi
```

You can find the name of the service into the docker-compose.yml file or in README if the documentation if up to date.

If you git pull sooner, before running the up command, run pull and build to make sure everything is up to date.

```bash
docker-compose pull && docker-compose build && docker-compose down && docker-compose up
```

You may notice a new keyword *down* ([keep reading it's coming...](#compose-down))

### compose down

> Stops containers and removes containers, networks, volumes, and images created by up. [More...][6]

As the documentation said, with this command you will stop everything and remove the containers.

Be careful, by default this command doesn't remove the data. If one of the services is a database. The down command will keep the data. If you want to remove data you need to specify the command with:

```
docker-compose down -v
```

# Cheatsheet

## I want to run the dependencies only

```bash
docker-compose pull && docker-compose up [List of dependencies I want to run separate with space]
```

## I want to run everything

```bash
docker-compose pull && docker-compose build && docker-compose up
```

## I want to clean everything

```bash
docker-compose down -v
```

[1]: https://docs.docker.com/glossary/ "Docker Glossary"
[2]: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/container-docker-introduction/docker-terminology "Microsoft Docker Terminology"
[3]: (https://docs.docker.com/compose/reference/pull/) "More about docker-compose pull"
[4]: (https://docs.docker.com/compose/reference/build/) "More about docker-compose build"
[5]: (https://docs.docker.com/compose/reference/up/) "More about docker-compose up"
[6]: (https://docs.docker.com/compose/reference/down/) "More about docker-compose down"
