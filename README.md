## Structure
### Domain
All domain logic is contained within the `Domain` folder. This includes:
- Entites - Contains the entities used for persistent storage and domain logic
- Enums - Contains the enum types used in the domain

### Application
The `Application` folder contains the application logic, which is divided into several subfolders:
- Common - Contains the common application logic, such as interfaces and base classes, all that is shared across the modules
  - Services - Contains the application services that are shared across the modules
  - Interfaces - Contains the common interfaces for the application services
  - Repositories - Contains the common repository interfaces and implementations used for accessing the data in the application
- Modules - Contains the application modules
  - [Name]
    - Models - Contains the data transfer objects (dtos) used for transferring data between layers
    - Services - Contains the application services that are specific to the module
      - Implementations - Contains the application service implementations for the module
    - Commands - Contains the CQSR commands used for modifying data in the application
    - Repositories - Contains the repository interfaces and implementations used for accessing the data in the application
      - Implementations - Contains the repository implementations for the module

### Infrastructure
The `Infrastructure` folder contains the infrastructure logic for externals services such as email and database, which is divided into several subfolders:
- Database - Contains the database context
- Migrations - Contains the database migrations

### Web (Presentation)
- Controllers - Contains the API controllers
- Services - Contains services that are only used for presentation purposes, such as view models and API-specific logic

## Terminology

This project uses a couple of affixes to denote the type of files and their roles within the application:
- ****Entity**: A persistent storage entity, typically representing a table row in a database
- ****Dto**: A data transfer object, used for transferring data between layers
- ****Vm**: A view model, used for representing data in the UI

The build of the name will be as follows:
- **Entity**: `[Name]Entity`
- **Dto**: `[Name]Dto`
- **Vm**: `[Name]Vm`

## Service implementation
To add services you can extend on one of the follwing interfaces to automatically register the service in the dependency injection container:
- `ITransientService` - Service with transient lifetime
- `IScopedService` - Service with scoped lifetime
- `ISingletonService` - Service with singleton lifetime

These 3 interfaces also implement IDisposable

## Default Services
- ICqrsSender (Application Layer) - This services handles all the CQRS commands and queries in the application. It is used to send commands and queries
