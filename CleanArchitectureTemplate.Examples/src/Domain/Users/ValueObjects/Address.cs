using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace SP.SampleCleanArchitectureTemplate.Domain.Users.ValueObjects
{
    public sealed class Address : ValueObject
    {
        private Address(string address1,
                       string city,
                       string country)
        {
            Address1 = address1;
            City     = city;
            Country  = country;
        }

        public string Address1 { get; }
        public string City     { get; }
        public string Country  { get; }


        public static Result<Address> Create(string address1,
                                    string city,
                                    string country)
        {
            if (string.IsNullOrWhiteSpace(address1))
            {
                return Result.Failure<Address>("Address should not be empty");
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                return Result.Failure<Address>("City should not be empty");
            }

            if (string.IsNullOrWhiteSpace(country))
            {
                return Result.Failure<Address>("Country should not be empty");
            }

            return Result.Success(new Address(address1, city, country));
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address1;
            yield return City;
            yield return Country;
        }
    }
}