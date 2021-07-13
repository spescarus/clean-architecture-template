using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using SP.SampleCleanArchitectureTemplate.Application.Services.Users.Models;
using SP.SampleCleanArchitectureTemplate.Application.Services.Users.Models.Responses;
using SP.SampleCleanArchitectureTemplate.Domain.Users;

namespace SP.SampleCleanArchitectureTemplate.Application.Services.Users
{
    public interface IUserService
    {
        Task<Result<UserCollectionResponse>> GetAllUsersAsync(int? limit,
                                                              int? offset);

        Task<Result<UserResponse>> GetUserByIdAsync(UserId             userId);
        Task<Result<UserResponse>> CreateUserAsync(CreateUserParameter parameter);

        Task<Result<User>> UpdateUserAsync(UpdateUserParameter parameter,
                                           UserId              userIdCaller);

        Task DeleteUserAsync(UserId userId,
                             UserId userIdCaller);
    }
}
