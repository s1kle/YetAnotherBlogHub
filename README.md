# ***Yet Another Blog Hub***

>[!NOTE]
>*Swagger*:
>
>![swagger screen](https://i.imgur.com/MNsjnZ4.png)
>
><hr>
>
>*Frontend*:
>
>![disign is too cringe on this stage so no screens attached](https://i.imgur.com/G4IKg39.png?1)
>
><hr>
>
>*Blog ERD*:
>
>![Blog ERD screen](https://i.imgur.com/rxIk1sf.png)

>[!IMPORTANT]
>### This is my pet-project to show my skills in c# w/ asp.net (and some ts w/ angular) 
1. *Idea*:
  - Fullstack application for working with blogs
2. *Tech stack*:
  - Frontend:
    - Angular
    - Bootstrap
  - Backend:
    - data:
      - AutoMapper
      - FluentValidation
      - MediatR
    - tests:
      - AutoFixture
      - FakeItEasy
      - FluentAssertions
      - xUnit
    - api:
      - asp.net 7
      - Redis
      - PostgreSQL
      - Serilog
      - Swagger
    - identity:
      - IdentityServer4
3. *Functionality*:
  - Frontend:
    - Communicates with backend APIs to perform CRUD operations on blogs, user authentication flows for login/register using IdentityServer, basic UI with Bootstrap
  - Backend:
    - Domain layer with blog entity, CQRS implementation for data layer to handle queries and commands, Input validation and mapping, ASP.NET Core Web API with blog controller, IdentityServer4 for authentication and authorization, Integration testing using xUnit
4. *Future Scope*:
  - [ ] add: tags
  - [ ] add: search by title/tag
  - [ ] add: comments
  - [ ] enhance ui/ux

>[!WARNING]
>If you want to start app:
>
>1. ***Rename*** ***appsettings.template.json*** to ***appsettings.json*** (api/identity) and provide your configuration
>
>1. ***Configure*** IdentityServer by providing your own details in the configuration class at ***Config/IdentityServerConfiguration.cs*** file (Template available in ***Config/IdentityServerConfiguration-Template.cs***)
> 
>1. No ***entry point*** yet, so you have to build/start projects (***frontend/angular_client*** | ***backend/api*** | ***backend/identity***) and setup database manually
