using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class ProfileRepository(SocialNetworkDbContext dbContext)
    : RepositoryBase<Profile>(dbContext), IProfileRepository
{
}