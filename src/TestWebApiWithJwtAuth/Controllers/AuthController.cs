using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWebApiWithJwtAuth.Authenticate;
using TestWebApiWithJwtAuth.Domain;
using TestWebApiWithJwtAuth.Services;

namespace TestWebApiWithJwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtIssuerOptions _jwtIssuerOptions;
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
       //public AuthController(IUserService userService, JwtIssuerOptions jwtIssuerOptions)
        {
            _userService = userService;
            //_jwtIssuerOptions = jwtIssuerOptions;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public ActionResult RegisterUser(RegisterUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login))
            {
                ModelState.AddModelError("Login", "value is null or empty");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                ModelState.AddModelError("email", "value is null or empty");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                ModelState.AddModelError("password", "value is null or empty");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

#pragma warning disable CS8604
            if (_userService.IsUserExist(request.Login))
#pragma warning restore CS8604
            {
                return BadRequest();
            }

            _userService.SaveUser(
#pragma warning disable CS8601
                new User { Login = request.Login, Email = request.Email, Password = request.Password});
#pragma warning restore CS8601

            return Ok();
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ActionResult Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login))
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest();
            }

            var user = _userService.GetUserByLoginPassword(request.Login, request.Password);

            /*var token = GetToken(user);

            Response.Headers.Add("Authorization", token);*/

            return Ok(user);
        }





        private string GetToken(User user)
        {
            var identity = GetIdentity(user);

            var jwt = new JwtSecurityToken(
                _jwtIssuerOptions.Issuer,
                _jwtIssuerOptions.Audience,
                identity.Claims,
                _jwtIssuerOptions.NotBefore,
                _jwtIssuerOptions.Expiration,
                _jwtIssuerOptions.SigningCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>()
            {
                new(JwtClaimIdentifiers.Id,  user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, _jwtIssuerOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.TimeOfDay.Ticks.ToString(), ClaimValueTypes.Integer64),
            };
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");

            return claimsIdentity;
        }
    }

    public class LoginRequest
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
    }

    public class RegisterUserRequest
    {
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
} 