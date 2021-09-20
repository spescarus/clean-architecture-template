<br />
<p align="center">
  <h3 align="center">Micro Service Template</h3>
</p>

# Table of Contents

- [Table of Contents](#table-of-contents)
- [About The Project](#about-the-project)
- [Getting started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Setup](#setup)
  - [Run](#run)
  - [Stop and clean](#stop-and-clean)
- [Contributing](#contributing)
- [Docker Compose content](#docker-compose-content)
- [Acknowledgements](#acknowledgements)

# About The Project

This is a template to create a new Project. This template contains the minimal project already configured with the "basics".
The documentation to initialize this template is [here](docs/Initialize-Template.md).

# Getting started

## Prerequisites

This project uses [Docker](https://www.docker.com/) and Docker compose to be run.

For linux: [Docker install](https://docs.docker.com/engine/install/ubuntu/)
or for Window and Mac OS: [Docker Install](https://docs.docker.com/desktop/)

After the installation you should be able to run this command and have a similar result:
```sh
> docker version
Client: Docker Engine - Community
 Cloud integration: 1.0.7
 Version:           20.10.2
 API version:       1.41
 Go version:        go1.13.15
 Git commit:        2291f61
 Built:             Mon Dec 28 16:14:16 2020
 OS/Arch:           windows/amd64
 Context:           default
 Experimental:      true

Server: Docker Engine - Community
 Engine:
  Version:          20.10.2
  API version:      1.41 (minimum version 1.12)
  Go version:       go1.13.15
  Git commit:       8891c58
  Built:            Mon Dec 28 16:15:28 2020
  OS/Arch:          linux/amd64
  Experimental:     false
 containerd:
  Version:          1.4.3
  GitCommit:        269548fa27e0089a8b8278fc4fc781d7f65a939b
 runc:
  Version:          1.0.0-rc92
  GitCommit:        ff819c7e9184c13b7c2607fe6c30ae19403a7aff
 docker-init:
  Version:          0.19.0
  GitCommit:        de40ad0
```

You are now ready to run the app !

For more information about [Docker and how to use it](docs/Docker-how-to.md)

## Setup

Nothing to setup. Good to go !

## Run

The service can be run with this simple command at the root of the repository
```bash
> docker-compose pull
> docker-compose build
> docker-compose up
```

- The *pull* command will get the latest image of each external dependency (like git pull)
- The *build* command will build the service and local dependencies
- The *up* command will start the service and all the dependencies

Browse the url at [localhost:5000](http://localhost:5000/swagger). You should have the swagger of the service.

## Stop and clean
In order to stop the service, use CTRL+C in the terminal. If you close the terminal, the container will continue to run in background.

If you want to clean the service and remove all data associated like database and so on, run this command:
```bash
> docker-compose down -v
```
If you want only remove container but not the data:
```bash
> docker-compose down
```
[More information](https://docs.docker.com/compose/reference/down/)

# Contributing

1. Clone the project
2. Check if you have the required version of dotnet core. See the version in global.json file.
3. Open the project in Visual Studio
4. Start the dependencies with the command 
   ```bash
   > docker-compose up webapi_database
   ```
5. Create your Feature Branch (`git checkout -b feature/NewFeature`)
6. Commit your Changes (`git commit -m 'Add some NewFeature'`)
7. Push to the Branch (`git push origin feature/NewFeature`)
8. Open a Pull Request

# Docker Compose content

|Key                    |Description                          |Port|
|-----------------------|-------------------------------------|----|
|webapi                 |The webapi service                   |5000|
|webapi_database        |The database associate               |5432|

# Acknowledgements
* [Asp.net core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0)
* [EntityFramework core](https://docs.microsoft.com/en-us/ef/core/)
* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/)
