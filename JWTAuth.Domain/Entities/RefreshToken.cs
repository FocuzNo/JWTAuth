namespace JWTAuth.Domain.Entities
{
    public class RefreshToken
    {
        public required string Token { get; set; }
        public DateTime CreateToken { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
    }
}
