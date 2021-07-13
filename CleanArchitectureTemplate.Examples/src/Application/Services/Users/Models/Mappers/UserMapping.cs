using System.Collections.Generic;
using AutoMapper;
using SP.SampleCleanArchitectureTemplate.Application.Extensions.TaskExtensions;
using SP.SampleCleanArchitectureTemplate.Application.Services.Users.Models.Responses;
using SP.SampleCleanArchitectureTemplate.Domain.Users;
using SP.SampleCleanArchitectureTemplate.Domain.Users.ValueObjects;

namespace SP.SampleCleanArchitectureTemplate.Application.Services.Users.Models.Mappers
{
    public class UserMapping : Profile  
    {
        public UserMapping()
        {
            CreateMap(typeof(IPartialCollection<>), typeof(UserCollectionResponse));
            CreateMap(typeof(ICollection<>),        typeof(UserCollectionResponse));

            CreateMap<User, UserResponse>()
               .ForMember(p => p.UserName,  map => map.MapFrom(p => p.UserName.Value))
               .ForMember(p => p.FirstName, map => map.MapFrom(p => p.Name.First))
               .ForMember(p => p.LastName,  map => map.MapFrom(p => p.Name.Last))
               .ForMember(p => p.Email,     map => map.MapFrom(p => p.Email.Value))
               .ForMember(p => p.Address,   map => map.MapFrom(p => p.Address));

            CreateMap<Address, AddressResponse>();
        }
    }
}
