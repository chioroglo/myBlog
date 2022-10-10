namespace Common.Dto.Auth
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Token { get; set; }
    }
}