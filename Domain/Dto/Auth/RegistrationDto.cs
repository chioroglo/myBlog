using System.ComponentModel.DataAnnotations;

namespace Domain.Dto.Account
{
    public class RegistrationDto
    {
        public string Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Password { get; set; }
    }
}
