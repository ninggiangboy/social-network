using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public sealed class GetFriendsIdSpec : Specification<Follow, string>
{
    public GetFriendsIdSpec(string profileId)
    {
        Query.Where(f => f.FolloweeId == profileId);
        Query.Where(f => f.Followee.Following!.Any(follow => follow.FollowerId == follow.FolloweeId));
        Query.Select(f => f.FollowerId);
    }
}