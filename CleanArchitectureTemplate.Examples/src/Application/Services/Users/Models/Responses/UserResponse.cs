using SP.SampleCleanArchitectureTemplate.Application.Base.Models;
using SP.SampleCleanArchitectureTemplate.Domain.Users;

namespace SP.SampleCleanArchitectureTemplate.Application.Services.Users.Models.Responses
{
    public class UserResponse : TimeStampedModel
    {
        public UserId          Id        { get; set; }
        public string          UserName  { get; set; }
        public string          FirstName { get; set; }
        public string          LastName  { get; set; }
        public string          Email     { get; set; }
        public AddressResponse Address   { get; set; }
    }
}
