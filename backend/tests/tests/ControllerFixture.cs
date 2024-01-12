using BlogHub.Api.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace BlogHub.Tests;

public class ControllerFixture<T> : BaseControllerFixture where T: BaseController
{
    public T Controller { get; init; }
    public ControllerFixture(ServiceProvider serviceProvider, T controller) 
        : base(serviceProvider)
    {
        Controller = controller;
        ChangeUser();
    }

    public override void ChangeUser()
    {
        var httpContext = ChangeContext();
        Controller.ControllerContext.HttpContext = httpContext;
    }
}