using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public sealed class GetFollowersIdSpec : Specification<Follow, string>
{
    public GetFollowersIdSpec(string authorId)
    {
        Query.Where(f => f.FolloweeId == authorId);
        Query.Select(f => f.FollowerId);
    }
}