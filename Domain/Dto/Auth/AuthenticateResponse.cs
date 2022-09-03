namespace Domain.Dto.Account
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Token { get; set; }
    }
}