using CSharpFunctionalExtensions;
using SP.SampleCleanArchitectureTemplate.Domain.Base;
using SP.SampleCleanArchitectureTemplate.Domain.Users.ValueObjects;

namespace SP.SampleCleanArchitectureTemplate.Domain.Users
{
    public sealed class User : BasicEntity<UserId>
    {
        public User(UserName       userName,
                    Name           name,
                    Email          email,
                    Maybe<Address> address)
        {
            Id       = UserId.New();
            UserName = userName;
            Name     = name;
            Email    = email;
            Address = address.HasNoValue
                          ? null
                          : address.Value;
        }

        public UserName UserName { get; }
        public Name     Name     { get; set; }
        public Email    Email    { get; set; }
        public Address  Address  { get; set; }
    }
}
