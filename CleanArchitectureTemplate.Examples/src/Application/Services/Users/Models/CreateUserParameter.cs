namespace SP.SampleCleanArchitectureTemplate.Application.Services.Users.Models
{
    public sealed class CreateUserParameter
    {
        public string           UserName  { get; set; }
        public string           FirstName { get; set; }
        public string           LastName  { get; set; }
        public string           Email     { get; set; }
        public AddressParameter Address   { get; set; }
    }
}
