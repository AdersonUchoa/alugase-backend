namespace Application.Responses.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string Login { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}
