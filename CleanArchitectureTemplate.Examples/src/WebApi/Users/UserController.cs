using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.SampleCleanArchitectureTemplate.Application.Services.Users;
using SP.SampleCleanArchitectureTemplate.Application.Services.Users.Models;
using SP.SampleCleanArchitectureTemplate.Application.Services.Users.Models.Responses;
using SP.SampleCleanArchitectureTemplate.Domain.Users;
using SP.SampleCleanArchitectureTemplate.WebApi.Base;

namespace SP.SampleCleanArchitectureTemplate.WebApi.Users
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/users")]
    [ExcludeFromCodeCoverage]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UserController : BaseController
    {
        private readonly IUserService      _userService;
        private readonly IExecutionContext _executionContext;
        private readonly IMapper           _mapper;

        public UserController(IUserService      userService,
                              IExecutionContext executionContext,
                              IMapper           mapper)
        {
            _userService      = userService;
            _executionContext = executionContext;
            _mapper           = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserResponse),  StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsers([FromQuery] int? limit  = null,
                                                           [FromQuery] int? offset = null)
        {
            var result = await _userService.GetAllUsersAsync(limit, offset);

            return FromResult(result);
        }

        [HttpGet("{userId:required}")]
        [ProducesResponseType(typeof(UserResponse),  StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(UserId userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);

            return FromResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserResponse),  StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserParameter parameter)
        {
            var result = await _userService.CreateUserAsync(parameter);

            return FromResult(result);
        }

        [HttpPut("{userId:required}")]
        [ProducesResponseType(typeof(UserResponse),  StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<UserResponse> UpdateUser(UserId                         userId,
                                                   [FromBody] UpdateUserParameter parameter)
        {

            var user = await _userService.UpdateUserAsync(parameter, userId);

            return _mapper.Map<UserResponse>(user);
        }

        [HttpDelete("{userId:required}")]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse),   StatusCodes.Status404NotFound)]
        public async Task<NoContentResult> DeleteFormTemplate(UserId userId)
        {
            var userIdCaller = _executionContext.UserIdCaller;

            await _userService.DeleteUserAsync(userId, userIdCaller);

            return NoContent();
        }
    }
}
