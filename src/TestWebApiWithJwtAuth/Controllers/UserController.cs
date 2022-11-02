using Microsoft.AspNetCore.Mvc;
using TestWebApiWithJwtAuth.Domain;
using TestWebApiWithJwtAuth.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestWebApiWithJwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _logger = logger;
            _userService = userService;
        }


        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            User? user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return new ObjectResult(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                ModelState.AddModelError("Name", "Name is null or empty");
            }

            if (string.IsNullOrWhiteSpace(user.Surname))
            {
                ModelState.AddModelError("Surname", "Surname is null or empty");
            }

            if (string.IsNullOrWhiteSpace(user.Phone))
            {
                ModelState.AddModelError("Phone", "Phone is null or empty");
            }

            _userService.SaveUser(user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User user)
        {
        }
    }
}
