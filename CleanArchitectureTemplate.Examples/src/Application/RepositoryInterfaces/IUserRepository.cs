using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using SP.SampleCleanArchitectureTemplate.Application.Extensions.TaskExtensions;
using SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.SampleCleanArchitectureTemplate.Domain.Users;
using SP.SampleCleanArchitectureTemplate.Domain.Users.ValueObjects;

namespace SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces
{
    public interface IUserRepository : IRepository<User, UserId>
    {
        public Task<IPartialCollection<User>> GetAllUsers(int? limit,
                                                          int? offset);

        Task<Maybe<User>> GetByUserName(UserName userName);
    }
}
