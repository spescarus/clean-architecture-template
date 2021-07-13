using SP.CleanArchitectureTemplate.Domain.Users;

namespace SP.CleanArchitectureTemplate.Application.RepositoryInterfaces.Generics
{
    public interface IExecutionContext
    {
        UserId UserIdCaller   { get; }
    }
}
