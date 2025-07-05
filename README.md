## Structure
### Domain
All domain logic is contained within the `Domain` folder. This includes:
- Entities - Contains the entities used for persistent storage and domain logic
- Enums - Contains the enum types used in the domain

### Application
The `Application` folder contains the application logic, which is divided into several subfolders:
- Common - Contains the common application logic, such as interfaces and base classes, all that is shared across the modules
  - Services - Contains the application services that are shared across the modules
  - Interfaces - Contains the common interfaces for the application services
  - Repositories - Contains the common repository interfaces and implementations used for accessing the data in the application
  - Seeders - Contains the seeders used for seeding the database with initial data
- Modules - Contains the application modules
  - [Name] 
    - Models - Contains the data transfer objects (dtos) used for transferring data between layers
    - Services - Contains the application services that are specific to the module
      - Implementations - Contains the application service implementations for the module
    - Commands - Contains the CQSR commands used for modifying data in the application
    - Repositories - Contains the repository interfaces and implementations used for accessing the data in the application
      - Implementations - Contains the repository implementations for the module
    - Seeders - Contains the seeders used for seeding the database with initial data for the module

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
- **ViewModel**: `[Name]ViewModel`

## Service implementation
To add services you can extend on one of the following interfaces to automatically register the service in the dependency injection container:
- `ITransientService` - Service with transient lifetime
- `IScopedService` - Service with scoped lifetime
- `ISingletonService` - Service with singleton lifetime

These 3 interfaces also implement IDisposable

## Default Services
- ICqrsSender (Application Layer) - This services handles all the CQRS commands and queries in the application. It is used to send commands and queries

## Domain Events
Domain events are used to notify the application of changes in the domain. They are used to decouple the domain logic from the application logic. The domain events are implemented using the `IDomainEvent` interface, which is used to define the domain events in the application.

## Validators
You can use in the application layer 2 types of validators:
- `IValidatorHandler<T>` - Used to validate commands and queries in the application layer.
- `AbstractValidator<T>` - FluentValidation validator used to validate the data in the application layer.

When you are using both methods of validation, the `AbstractValidator<T>` will be executed first, and then the `IValidatorHandler<T>` will be used to validate the data in the command or query.
It is recommended to use the `AbstractValidator<T>` fluent validation for validation rules, and the `IValidatorHandler<T>` when the fluent validation is not enough.

## Disclaimer
This project is a sample project and is not intended for production use. It is meant for some theorie testing and own concepts around DDD and CQRS. The code is provided as-is and without warranty of any kind. Use it at your own risk.