using System.IdentityModel.Tokens.Jwt;
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

[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public BlogController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet][Authorize]
    public async Task<ActionResult<BlogListVm>> GetAll() 
    {
        var userId = GetCurrentUserId();

        var query = new GetBlogListQuery() 
        {
            UserId = userId
        };

        var responce = await _mediator.Send(query);

        return Ok(responce);
    }

    [HttpGet("{id}")][Authorize]
    public async Task<ActionResult<BlogVm>> GetById(Guid id)
    {
        var userId = GetCurrentUserId();

        var query = new GetBlogQuery() 
        {
            UserId = userId,
            Id = id
        };

        var responce = await _mediator.Send(query);

        return Ok(responce);
    }

    [HttpPost][Authorize]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateBlogDto dto)
    {
        var userId = GetCurrentUserId();

        var mappedDto = _mapper.Map<CreateBlogCommand>(dto);
        var command = mappedDto with { UserId = userId };

        var blogId = await _mediator.Send(command);

        return Ok(blogId);
    }

    [HttpPut("{id}")][Authorize]
    public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] UpdateBlogDto dto)
    {
        var userId = GetCurrentUserId();

        var mappedDto = _mapper.Map<UpdateBlogCommand>(dto);
        var command = mappedDto with { Id = id, UserId = userId };

        var blogId = await _mediator.Send(command);

        return Ok(blogId);
    }

    [HttpDelete("{id}")][Authorize]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var userId = GetCurrentUserId();

        var command = new DeleteBlogCommand
        {
            Id = id,
            UserId = userId
        };

        var blogId = await _mediator.Send(command);

        return Ok(blogId);
    }

    private Guid GetCurrentUserId()
    {
        var header = HttpContext.Request.Headers["Authorization"]
            .ToString();

        var token = header.Substring("Bearer ".Length);

        if (string.IsNullOrEmpty(token)) 
            throw new ArgumentException($"Token parsing error. {token}");

        var handler = new JwtSecurityTokenHandler();
        var claim = handler.ReadJwtToken(token)?.Payload.Sub 
            ?? throw new ArgumentException($"Token reading error. {token}");

        var id = Guid.Parse(claim);

        return id;
    }
}