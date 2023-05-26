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

        public CRUDController(ICRUDRepository crudRepository)
        {
            _crudRepository = crudRepository;
        }

        [HttpGet("GetUsers"), Authorize]
        public ActionResult GetUsers()
        {
            return Ok();
        }
    }
}
