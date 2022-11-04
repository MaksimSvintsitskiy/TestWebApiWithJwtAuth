using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWebApiWithJwtAuth.Authenticate;
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
            if (!TryCheckUserClaims(id))
            {
                return Forbid();
            }

            User? user = _usersService.GetUserById(id);
            if (user == null)
            {
                return Forbid();
            }

            return new ObjectResult(user);
        }
        

        [Authorize]
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            if (!TryCheckUserClaims(user.Id))
            {
                return Forbid();
            }

            var userById = _usersService.GetUserById(user.Id);

            if (userById == null)
            {
                return Forbid();
            }

            #region validate user
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
            #endregion


            _usersService.SaveUser(user);

            return Ok();
        }

        private bool TryCheckUserClaims(int userId)
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(i => string.Equals(i.Type, JwtClaimIdentifiers.Id));

            if (userIdClaim == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(userIdClaim.Value)
                || !int.TryParse(userIdClaim.Value, out var userIdFromClaim)
                || userIdFromClaim == 0
                || userId != userIdFromClaim)
            {
                return false;
            }

            return true;
        }
    }
}
