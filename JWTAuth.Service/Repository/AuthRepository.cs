using JWTAuth.Domain;
using JWTAuth.Domain.Entities;
using JWTAuth.Service.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuth.Service
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        private static User user = new();

        public AuthRepository(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public void RegisterAccount(RegisterUser registerUser)
        {
            string passworHash = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);

            user.Username = registerUser.Username;
            user.PasswordHash = passworHash;

            _dataContext.Add(user);
            _dataContext.SaveChanges();
        }

        public string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
            new Claim(ClaimTypes.Name, user.Username!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:Token").Value!));

            var signCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: signCred
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
