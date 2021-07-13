using SP.CleanArchitectureTemplate.Application.Exceptions;

namespace Infrastructure.Exceptions
{
    public class UserIdRequiredInTokenException : TechnicalException
    {
        public UserIdRequiredInTokenException()
           : base($"The current user doesn't have an idea in the token")
        {

        }
    }
}
