using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SP.SampleCleanArchitectureTemplate.Application.Base;
using SP.SampleCleanArchitectureTemplate.Application.Exceptions;
using SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces;
using SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.SampleCleanArchitectureTemplate.Application.Services.Users.Models;
using SP.SampleCleanArchitectureTemplate.Application.Services.Users.Models.Responses;
using SP.SampleCleanArchitectureTemplate.Domain.Users;
using SP.SampleCleanArchitectureTemplate.Domain.Users.ValueObjects;

namespace SP.SampleCleanArchitectureTemplate.Application.Services.Users
{
    public sealed class UserService : Service, IUserService
    {
        private readonly IUserRepository    _userRepository;
        private readonly IValidationService _validationService;
        private readonly IUnitOfWork        _unitOfWork;
        private readonly IMapper            _mapper;

        public UserService(IUserRepository      userRepository,
                           IValidationService   validationService,
                           IUnitOfWork          unitOfWork,
                           IMapper              mapper,
                           ILogger<UserService> logger)
            : base(mapper, logger)
        {
            _userRepository    = userRepository;
            _validationService = validationService;
            _unitOfWork        = unitOfWork;
            _mapper            = mapper;
        }

        public async Task<Result<UserCollectionResponse>> GetAllUsersAsync(int? limit,
                                                                           int? offset)
        {
            var users = await _userRepository.GetAllUsers(limit, offset);

            var userCollectionResponse = _mapper.Map<UserCollectionResponse>(users);

            return Result.Success(userCollectionResponse);
        }

        public async Task<Result<UserResponse>> GetUserByIdAsync(UserId userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return Result.Failure<UserResponse>($"User with id {userId} was not found");
            }

            var userResponse = _mapper.Map<UserResponse>(user);

            return Result.Success(userResponse);
        }

        public async Task<Result<UserResponse>> CreateUserAsync(CreateUserParameter parameter)
        {
            var scope = await _unitOfWork.CreateScopeAsync();

            Result<UserName>       userNameOrError = UserName.Create(parameter.UserName);
            Result<Email>          emailOrError    = Email.Create(parameter.Email);
            Result<Name>           nameOrError     = Name.Create(parameter.FirstName, parameter.LastName);
            Result<Maybe<Address>> address         = CreateAddress(parameter.Address);
            var                    result          = Result.Combine(userNameOrError, emailOrError, address);

            if (result.IsFailure)
            {
                return result.ConvertFailure<UserResponse>();
            }

            var byUserName = await _userRepository.GetByUserName(userNameOrError.Value);

            if (byUserName.HasValue)
            {
                return Result.Failure<UserResponse>($"Username {parameter.UserName} is already in use");
            }

            var user = new User(userNameOrError.Value, nameOrError.Value, emailOrError.Value, address.Value);
            await _validationService.ValidateAsync(user);
            await _userRepository.AddAsync(user, user.Id);

            await scope.SaveAsync();
            await scope.CommitAsync();

            var userResponse = _mapper.Map<UserResponse>(user);

            return Result.Success(userResponse);
        }

        public async Task<Result<User>> UpdateUserAsync(UpdateUserParameter parameter,
                                                        UserId              userIdCaller)
        {
            var scope = await _unitOfWork.CreateScopeAsync();

            Result<Name>           nameOrError  = Name.Create(parameter.FirstName, parameter.LastName);
            Result<Email>          emailOrError = Email.Create(parameter.Email);
            Result<Maybe<Address>> address      = CreateAddress(parameter.Address);
            var                    result       = Result.Combine(nameOrError, emailOrError, address);

            if (result.IsFailure)
            {
                return result.ConvertFailure<User>();
            }

            var user = await _userRepository.GetByIdAsync(userIdCaller);

            if (user == null)
            {
                throw new EntityNotFoundException<User, UserId>(userIdCaller);
            }

            user.Name    = nameOrError.Value;
            user.Email   = emailOrError.Value;
            user.Address = address.Value.Value;
            
            await _validationService.ValidateAsync(user);
            await _userRepository.Update(user, userIdCaller);

            await scope.SaveAsync();
            await scope.CommitAsync();

            return Result.Success(user);
        }

        public async Task DeleteUserAsync(UserId userId,
                                          UserId userIdCaller)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            _userRepository.Delete(user, userIdCaller);

            await _unitOfWork.SaveAsync()
                             .ConfigureAwait(false);
        }

        private static Result<Maybe<Address>> CreateAddress(AddressParameter addressParam)
        {
            if (addressParam == null)
                return Result.Success(Maybe<Address>.None);

            return Address.Create(addressParam.Address1, addressParam.City, addressParam.Country)
                          .Map(address => (Maybe<Address>) address);
        }
    }
}
