using System.Threading.Tasks;

namespace SP.SampleCleanArchitectureTemplate.Application.Base
{
    public interface IValidationService
    {
        Task ValidateAsync<TEntity>(TEntity entity)
            where TEntity : class;
    }
}
