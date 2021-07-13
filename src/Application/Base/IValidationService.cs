using System.Threading.Tasks;

namespace SP.CleanArchitectureTemplate.Application.Base
{
    public interface IValidationService
    {
        Task ValidateAsync<TEntity>(TEntity entity)
            where TEntity : class;
    }
}
