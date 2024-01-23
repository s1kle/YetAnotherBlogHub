# ***Yet Another Blog Hub***

>[!NOTE]
>*Swagger*:
>
>![swagger screen](https://i.imgur.com/JNa96XP.png)
>![swagger screen](https://i.imgur.com/GPxMCVj.png)
>
><hr>
>
>*Data ERD*:
>
>![Blog ERD screen](https://i.imgur.com/zyix2a4.png)
>
>Client
>
>![BlogList Page](https://i.imgur.com/DMxFKB2.jpg)
>![Blog Page](https://i.imgur.com/tCXV8fd.jpg)

>[!IMPORTANT]
>### This is my pet-project to show my skills in c# w/ asp.net (and some ts w/ angular) 
1. *Idea*:
  - Fullstack application for working with articles
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
>Requirements: Docker
>If you want to start backend:
>
>1. ***Configure*** IdentityServer by providing your own details in the configuration class at ***Config/IdentityServerConfiguration.cs*** file (Template available in ***Config/IdentityServerConfiguration-Template.cs***)
> 
>1. Go to the ***backend*** directory and run the PowerShell (.ps1) or Bash (.sh) script.
>- For regular use: run `.\up.ps1` or `.\up.sh`
>- For development (rebuilding api/identity containers on host file save): run `.\watch.ps1` or `.\watch.sh`
>- To initialize the database context, run:
>```
>docker exec -it api dotnet ef database update
>docker exec -it identity dotnet ef database update
>```
