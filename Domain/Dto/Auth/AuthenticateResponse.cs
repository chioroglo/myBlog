namespace Domain.Dto.Account
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public byte[]? Avatar { get; set; }

        public DateTime LastActivity { get; set; }

        public string Token { get; set; }
    }
}
