using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWebApiWithJwtAuth.Domain;
using TestWebApiWithJwtAuth.Services;

namespace TestWebApiWithJwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            User? user = _usersService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return new ObjectResult(user);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            var userById = _usersService.GetUserById(user.Id);

            if (userById == null)
            {
                return BadRequest("Unknown userId");
            }

            if (string.IsNullOrWhiteSpace(user.Name))
            {
                ModelState.AddModelError("Name", "value is null or empty");
            }

            if (string.IsNullOrWhiteSpace(user.Surname))
            {
                ModelState.AddModelError("Surname", "value is null or empty");
            }

            if (string.IsNullOrWhiteSpace(user.Phone))
            {
                ModelState.AddModelError("Phone", "value is null or empty");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                ModelState.AddModelError("Email", "value is null or empty");
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                ModelState.AddModelError("Password", "value is null or empty");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _usersService.SaveUser(user);

            return Ok();
        }
    }
}
