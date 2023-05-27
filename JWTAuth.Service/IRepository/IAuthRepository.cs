using JWTAuth.Domain.Entities;

namespace JWTAuth.Service.IRepository
{
    public interface IAuthRepository
    {
        void RegisterAccount(RegisterUser registerUser);
        string GenerateToken(User user);
        RefreshToken GenerateRefreshToken();
    }
}
