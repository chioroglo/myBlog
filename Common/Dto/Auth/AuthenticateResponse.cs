namespace Common.Dto.Auth
{
    public record AuthenticateResponse
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Token { get; set; }

        public DateTime AuthorizationExpirationDate { get; set; }
    }
}