using System.Security.Claims;
using BlogHub.Api.Extensions;
using BlogHub.Data.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

public abstract class BaseBlogController : ControllerBase
{
    protected IMediator Mediator { get; }
    protected Guid UserId =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    protected BaseBlogController(IMediator mediator) =>
        Mediator = mediator;

    protected GetBlogListQuery ParseGetListDto(GetListDto dto, Guid? userId = null)
    {
        return new GetBlogListQuery()
        {
            UserId = userId,
            Size = dto.Size,
            Page = dto.Page
        }
            .ApplySearchFilter(dto.SearchQuery, dto.SearchProperties)
            .ApplySortFilter(dto.SortProperty, dto.SortDirection);
    }
}