using Application.DTOs.Posts;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mappers;

public class PostMapper : Profile
{
    public PostMapper()
    {
        CreateMap<PostCreateRequest, Post>();
        CreateMap<PostUpdateRequest, Post>();
    }
}