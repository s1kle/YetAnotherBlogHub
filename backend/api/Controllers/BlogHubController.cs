using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

[Route("api")]
public abstract class BlogHubController : ControllerBase
{
    protected IMediator Mediator { get; }
    protected Guid UserId =>
        Guid.Parse("0fe5fabf-67d0-5ba8-9020-a29d963763d6");
        // Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    protected BlogHubController(IMediator mediator) =>
        Mediator = mediator;
}