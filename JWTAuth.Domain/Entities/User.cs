namespace JWTAuth.Domain.Entities
{
    public class User : BaseEntity
    {
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
