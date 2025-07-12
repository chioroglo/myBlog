using Bogus;
using MyBlog.Common.Dto.Auth;

namespace MyBlog.FunctionalTests.Utils.Fakers;
internal class RegistrationDtoFaker : Faker<RegistrationDto>
{
    public RegistrationDtoFaker()
    {
        RuleFor(u => u.Username, f => f.Internet.UserName());
        RuleFor(u => u.FirstName, f => f.Name.FirstName());
        RuleFor(u => u.LastName, f => f.Name.LastName());
        RuleFor(u => u.Password, f => f.Internet.Password());
    }
}
