using System.Security.Claims;
using AutoMapper;
using BlogHub.Data.Commands.Create;
using BlogHub.Data.Commands.Delete;
using BlogHub.Data.Commands.Update;
using BlogHub.Data.Queries.Get;
using BlogHub.Data.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private Guid _userId =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public BlogController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet][AllowAnonymous]
    public async Task<ActionResult<BlogListVm>> GetAll([FromQuery] GetListDto dto)
    {
        var query = ParseGetListDto(dto, null);

        var response = await _mediator.Send(query);

        return Ok(response);
    }

    [HttpGet("All")]
    public async Task<ActionResult<BlogListVm>> GetAllByUserId([FromQuery] GetListDto dto)
    {
        var query = ParseGetListDto(dto, _userId);

        var response = await _mediator.Send(query);

        return Ok(response);
    }

    [HttpGet("Get/{id}")]
    public async Task<ActionResult<BlogVm>> GetById(Guid id)
    {
        var query = new GetBlogQuery()
        {
            UserId = _userId,
            Id = id
        };

        var response = await _mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateBlogDto dto)
    {
        var mappedDto = _mapper.Map<CreateBlogCommand>(dto);
        var command = mappedDto with { UserId = _userId };

        var blogId = await _mediator.Send(command);

        return Ok(blogId);
    }

    [HttpPut("Update/{id}")]
    public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] UpdateBlogDto dto)
    {
        var mappedDto = _mapper.Map<UpdateBlogCommand>(dto);
        var command = mappedDto with { Id = id, UserId = _userId };

        var blogId = await _mediator.Send(command);

        return Ok(blogId);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var command = new DeleteBlogCommand
        {
            Id = id,
            UserId = _userId
        };

        var blogId = await _mediator.Send(command);

        return Ok(blogId);
    }

    private GetBlogListQuery ParseGetListDto(GetListDto dto, Guid? userId)
    {
        return new ()
        {
            UserId = userId,
            Page = dto.Page,
            Size = dto.Size,
            SearchQuery = dto.SearchQuery ?? "",
            SearchProperties = dto.SearchProperties?.Split(' ') ?? new string[0],
            SortProperty = dto.SortProperty ?? "",
            SortDirection = dto.SortDirection switch
            {
                "desc" => true,
                _ => false
            }
        };
    }
}