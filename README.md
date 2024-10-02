# Book Reservation System

## About this solution

This is a layered startup solution which focused on seperation of concern.

### Pre-requirements

- [.NET 8.0+ SDK](https://dotnet.microsoft.com/download/dotnet)

### Configurations

The solution comes with a default configuration that works out of the box. However, you may consider to change the following configuration before running your solution:

- Check the `ConnectionStrings` in `appsettings.json` files under the `BookReservationSystem.WebApi` project and change it if you need.

### Before running the application

#### Create the Database

Run command in package manager console `Update-Database` to run the migrations and to create the initial database. This should be done in the first run.

### Solution structure

This is a layered monolith application that consists of the following applications:

- `BookReservationSystem.Domain`: A library application which applies the migrations and also seeds the initial data. It mainly focused on domain entities.
- `BookReservationSystem.Infrastructure`: A library application which consist of repository and it's interface.
- `BookReservationSystem.Applicationb`: A library application which consists of service and their interfaces.
- `BookReservationSystem.WebApi`: ASP.NET Core API application that is used to expose the APIs to the clients.

### Deploying the application

Please follow the `Technical Design Document` for the process of deployment.

### Additional info

`DockerFile` have added and builddocker.sh to build the docker file
`Docker-compose.yml` have added to containerized the database(postgres) and the project in local.
`Technical Design Document` have added for detail information which can be found under the Document folder.
`API Versioning` have implemented api versioning and currently it's version is 1.

`Note` I have tried to cover as much as possible. Any suggestions from you would be greatly appreciated :) .
