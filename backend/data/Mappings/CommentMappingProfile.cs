using BlogHub.Data.Comments.Queries.GetList;

namespace BlogHub.Data.Mappings;

internal sealed class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentVm>();
    }
}