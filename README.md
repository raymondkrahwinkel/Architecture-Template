## Structure
### Domain
All domain logic is contained within the `Domain` folder. This includes:
- Entites - Contains the entities used for persistent storage and domain logic
- Enums - Contains the enum types used in the domain

### Application
The `Application` folder contains the application logic, which is divided into several subfolders:
- Common - Contains the common application logic, such as interfaces and base classes, all that is shared across the modules
  - Services - Contains the application services that are shared across the modules
- Modules - Contains the application modules
  - Dto - Contains the data transfer objects used for transferring data between layers
  - Services - Contains the application services that are specific to the module
  - Commands - Contains the CQSR commands used for modifying data in the application

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