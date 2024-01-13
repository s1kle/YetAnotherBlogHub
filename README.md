# ***Yet Another Blog Hub***

>[!NOTE]
>*Swagger*:
>
>![swagger screen](https://i.imgur.com/JNa96XP.png)
>![swagger screen](https://i.imgur.com/GPxMCVj.png)
>
><hr>
>
>*Blog ERD*:
>
>![Blog ERD screen](https://i.imgur.com/BAOtdam.png)
>
>*Tag ERD*:
>
>![Tag ERD screen](https://i.imgur.com/tcdxquB.png)
>
>*Link ERD*:
>
>![Link ERD screen](https://i.imgur.com/0lwIunM.png)
>
>*User ERD*:
>
>![User ERD screen](https://i.imgur.com/T2yI1bV.png)
>
>*Сomment ERD*:
>
>![Сomment ERD screen](https://i.imgur.com/nBEzIeS.png)
>
>Client
>
>![BlogList Page](https://i.imgur.com/DMxFKB2.jpg)
>![Blog Page](https://i.imgur.com/tCXV8fd.jpg)

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
      - RabbitMQ.Client
3. *Functionality*:
  - Frontend:
    - Communicates with backend APIs to perform CRUD operations on blogs, user authentication flows for login/register using IdentityServer, basic UI with Bootstrap
  - Backend:
    - Domain layer with blog entity, CQRS implementation for data layer to handle queries and commands, Input validation and mapping, ASP.NET Core Web API with blog controller, IdentityServer4 for authentication and authorization, Integration testing using xUnit

>[!WARNING]
>If you want to start app:
>
>1. ***Rename*** ***appsettings.template.json*** to ***appsettings.json*** (api/identity) and provide your configuration
>
>1. ***Configure*** IdentityServer by providing your own details in the configuration class at ***Config/IdentityServerConfiguration.cs*** file (Template available in ***Config/IdentityServerConfiguration-Template.cs***)
> 
>1. No ***entry point*** yet, so you have to build/start projects (***frontend/angular_client*** | ***backend/api*** | ***backend/identity***) and setup database manually
