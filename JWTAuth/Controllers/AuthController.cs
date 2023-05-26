using Microsoft.AspNetCore.Mvc;
using JWTAuth.Service.IRepository;
using JWTAuth.Domain.Entities;
using JWTAuth.Domain;

namespace JWTAuth.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository, DataContext dataContext)
        {
            _authRepository = authRepository;
            _dataContext = dataContext;
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
            return Ok(token);
        }
    }
}
