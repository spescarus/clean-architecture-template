using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.SampleCleanArchitectureTemplate.Domain.Base;
using SP.SampleCleanArchitectureTemplate.Domain.Users;

namespace Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    public class ExecutionContext : IExecutionContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExecutionContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Use this for real applications that require authentication
        /// <exception cref="T:Infrastructure.Exceptions.UserNotAuthenticatedException" accessor="get">Throw exception when user is not authenticated.</exception>
        /// <exception cref="T:Infrastructure.Exceptions.UserIdRequiredInTokenException" accessor="get">Throw exception when user id is not Guid.</exception>
        /// </summary>
        public UserId UserIdCaller
        {
            get
            {
                if (_httpContextAccessor?.HttpContext?.User.Identity == null)
                {
                    throw new UserNotAuthenticatedException();
                }

                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    throw new UserNotAuthenticatedException();
                }

                if (!(_httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity identity))
                {
                    throw new UserNotAuthenticatedException();
                }

                var claimUser = identity.FindFirst("userId");

                if (claimUser == null)
                {
                    throw new UserNotAuthenticatedException();
                }

                if (string.IsNullOrEmpty(claimUser.Value) ||
                    !Guid.TryParse(claimUser.Value, out _))
                {
                    throw new UserIdRequiredInTokenException();
                }

                return claimUser.Value.ToEntityId<UserId>();
            }
        }
    }
}