using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

[Route("api")]
public abstract class BlogHubController : ControllerBase
{
    protected IMediator Mediator { get; }
    protected Guid UserId =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    protected BlogHubController(IMediator mediator) =>
        Mediator = mediator;
}