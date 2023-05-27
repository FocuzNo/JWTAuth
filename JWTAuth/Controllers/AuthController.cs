using Microsoft.AspNetCore.Mvc;
using JWTAuth.Service.IRepository;
using JWTAuth.Domain.Entities;
using JWTAuth.Domain;
using Microsoft.AspNetCore.Authorization;

namespace JWTAuth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        private readonly static User user = new();

        public AuthController(IAuthRepository authRepository, IUserRepository userRepository
            , DataContext dataContext)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _dataContext = dataContext;
        }

        [HttpGet("GetName"), Authorize]
        public ActionResult<string> GetName()
        {
            return Ok(_userRepository.GetName());
        }

        [HttpPost("Register")]
        public ActionResult Register(RegisterUser registerUser)
        {
            var users = _dataContext.Users.ToList();
            foreach(User user in users)
            {
                if(user.Username == registerUser.Username)
                {
                    return BadRequest("Such user already exists. " +
                        "Try again.");
                }
            }

            _authRepository.RegisterAccount(registerUser);
            return Ok(registerUser);
        }

        [HttpPost("Login")]
        public ActionResult Login(RegisterUser registerUser)
        {
            User? user =  
                 _dataContext.Users.FirstOrDefault(u => u.Username == registerUser.Username);

            if (user == null)
            {
                return BadRequest("Wrong username.");
            }

            if (!BCrypt.Net.BCrypt.Verify(registerUser.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password.");
            }

            var token = _authRepository.GenerateToken(user);

            var refreshToken = _authRepository.GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("RefreshToken"), Authorize]
        public ActionResult<string> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid refresh token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            var token = _authRepository.GenerateToken(user);

            var newRefreshToken = _authRepository.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.CreateToken;
            user.TokenExpires = newRefreshToken.Expires;
        }
    }
}
