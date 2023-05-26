namespace JWTAuth.Domain.Entities
{
    public class RegisterUser : BaseEntity
    {
        public required string? Username { get; set; }
        public required string? Password { get; set; }
    }
}
