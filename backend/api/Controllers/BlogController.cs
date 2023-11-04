using AutoMapper;
using BlogHub.Data.Commands.CreateBlog;
using BlogHub.Data.Queries.GetBlog;
using BlogHub.Data.Queries.GetBlogList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.BlogController;

[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly Guid _userId = Guid.Parse("d65371e9-7c76-4f18-a6d0-eb738fa1ed86");

    public BlogController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<BlogListVm>> GetAll() 
    {
        var query = new GetBlogListQuery() 
        {
            UserId = _userId
        };

        var responce = await _mediator.Send(query);

        return Ok(responce);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BlogVm>> GetById(Guid id)
    {
        var query = new GetBlogQuery() 
        {
            UserId = _userId,
            Id = id
        };

        var responce = await _mediator.Send(query);

        return Ok(responce);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateBlogModel createBlogDto)
    {
        var mappedDto = _mapper.Map<CreateBlogCommand>(createBlogDto);
        var command = mappedDto with { UserId = _userId };

        var blogId = await _mediator.Send(command);

        return Ok(blogId);
    }
}