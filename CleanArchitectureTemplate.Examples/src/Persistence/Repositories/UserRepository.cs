using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using SP.SampleCleanArchitectureTemplate.Application.Extensions.TaskExtensions;
using SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces;
using SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.SampleCleanArchitectureTemplate.Domain.Users;
using SP.SampleCleanArchitectureTemplate.Domain.Users.ValueObjects;
using SP.SampleCleanArchitectureTemplate.Persistence.Context;
using SP.SampleCleanArchitectureTemplate.Persistence.Repositories.Generics;
using SP.SampleCleanArchitectureTemplate.Persistence.TaskExtensions;

namespace SP.SampleCleanArchitectureTemplate.Persistence.Repositories
{
    public sealed class UserRepository : AppRepository<User, UserId>, IUserRepository
    {
        public UserRepository(AppDbContext context)
            : base(context)
        {
        }

        protected override void OnDelete(User       entity,
                                         UserId     userId,
                                         DeleteType deleteType)
        {
        }

        public async Task<IPartialCollection<User>> GetAllUsers(int? limit,
                                                                int? offset)
        {
            if (!limit.HasValue)
            {
                return await GetAllAsync()
                            .AsNoTracking()
                            .ToPartialCollection()
                            .ConfigureAwait(false);
            }

            offset ??= 0;

            return await GetAllAsync()
                        .Paginate(offset.Value, limit.Value)
                        .AsNoTracking()
                        .ConfigureAwait(false);
        }

        public async Task<Maybe<User>> GetByUserName(UserName userName)
        {
            var user = await GetFirstByAsync(p => p.UserName == userName.Value);

            return Maybe<User>.From(user);
        }
    }
}
