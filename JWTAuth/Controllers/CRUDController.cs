using JWTAuth.Domain;
using JWTAuth.Domain.Entities;
using JWTAuth.Service.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CRUDController : ControllerBase
    {
        private readonly ICRUDRepository _crudRepository;
        private readonly DataContext _dataContext;

        public CRUDController(ICRUDRepository crudRepository, DataContext dataContext)
        {
            _crudRepository = crudRepository;
            _dataContext = dataContext;
        }

        [HttpGet("GetUsers"), Authorize]
        public ActionResult GetUsers()
        {
            var user = _crudRepository.GetAllUsers();
            return Ok(user);
        }

        [HttpPost("CreateUser"), Authorize]
        public ActionResult CreateUser(RegisterUser registerUser)
        {
            var users = _dataContext.Users.ToList();
            foreach (User userList in users)
            {
                if (userList.Username == registerUser.Username)
                {
                    return BadRequest("Such user already exists. " +
                        "Try again.");
                }
            }

            _crudRepository.Create(registerUser);
            return Ok(registerUser);
        }

        [HttpPut("UpdateUser"), Authorize]
        public ActionResult UpdateUser(User user)
        {
            if (user == null)
            {
                return BadRequest("Such user already exists. " +
                         "Try again.");
            }

            _crudRepository.Update(user);
            return Ok(user);
        }

        [HttpDelete("DeleteUser"), Authorize]
        public ActionResult DeleteUser(string name)
        {
            if(name == null)
            {
                return BadRequest("No such user found. " +
                    "Try again.");
            }

             _crudRepository.Delete(name);
            return Ok();
        }
    }
}
