using System;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace SP.SampleCleanArchitectureTemplate.Domain.Users.ValueObjects
{
    public sealed class UserName : ValueObject<UserName>
    {
        public string Value { get; }

        private UserName(string value)
        {
            Value = value;
        }

        public static Result<UserName> Create(Maybe<string> userNameOrNothing)
        {
            return userNameOrNothing.ToResult("UserName should not be empty")
                                    .Tap(userName => userName.Trim())
                                    .Ensure(email => email              != string.Empty, "Username should not be empty")
                                    .Ensure(username => username.Length <= 200,          "Username is too long")
                                    .Ensure(IsUsername,                                  "Username must start with a letter, allow letter or number, length between 6 to 12")
                                    .Map(userName => new UserName(userName));
        }

        public static explicit operator UserName(string userName)
        {
            return Create(userName)
               .Value;
        }

        public static implicit operator string(UserName userName)
        {
            return userName.Value;
        }

        protected override bool EqualsCore(UserName other)
        {
            return Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }

        private static bool IsUsername(string username)
        {
            // start with a letter, allow letter or number, length between 6 to 12.
            const string pattern = "^[a-zA-Z][a-zA-Z0-9]{5,11}$";

            var regex = new Regex(pattern);
            return regex.IsMatch(username);
        }
    }
}
