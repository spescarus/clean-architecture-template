using SP.CleanArchitectureTemplate.Application.Exceptions;

namespace Infrastructure.Exceptions
{
    public class UserNotAuthenticatedException : TechnicalException
    {
        public UserNotAuthenticatedException()
            : base($"The current user is not authenticated")
        {

        }
    }
}
