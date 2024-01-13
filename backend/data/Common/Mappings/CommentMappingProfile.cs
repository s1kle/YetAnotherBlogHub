namespace BlogHub.Data.Common.Mappings;

internal sealed class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentVm>();
    }
}