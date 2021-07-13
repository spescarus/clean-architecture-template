using SP.SampleCleanArchitectureTemplate.Domain.Users;

namespace SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces.Generics
{
    public interface IExecutionContext
    {
        UserId UserIdCaller   { get; }
    }
}
