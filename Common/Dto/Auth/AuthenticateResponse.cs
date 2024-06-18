namespace Common.Dto.Auth
{
    public record AuthenticateResponse
    {
        public required int Id { get; set; }

        public required string Username { get; set; }

        public string Token { get; set; }

        public DateTime AuthorizationExpirationDate { get; set; }
    }
}