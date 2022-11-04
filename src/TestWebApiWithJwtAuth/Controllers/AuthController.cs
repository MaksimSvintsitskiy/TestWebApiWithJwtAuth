using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestWebApiWithJwtAuth.Authenticate;
using TestWebApiWithJwtAuth.Controllers.Requests;
using TestWebApiWithJwtAuth.Domain;
using TestWebApiWithJwtAuth.Services;

namespace TestWebApiWithJwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtAuthOptions _jwtAuthOptions;
        private readonly IUsersService _usersService;

        
       public AuthController(IUsersService usersService, JwtAuthOptions jwtAuthOptions)
        {
            _usersService = usersService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public ActionResult RegisterUser(RegisterUserRequest request)
        {
            #region validate request
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
            #endregion

            if (request.Login != null && _usersService.IsUserExist(request.Login))
            {
                return BadRequest("User already registered");
            }

            // in real app the password must be encrypted before save in db 
            _usersService.SaveUser(new User { Login = request.Login, Email = request.Email, Password = request.Password});

            return Ok();
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ActionResult Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login))
            {
                ModelState.AddModelError("Login", "value is null or empty");
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
            var user = _usersService.GetUserByLoginPassword(request.Login, request.Password);
#pragma warning restore CS8604

            if (user == null)
            {
                return BadRequest();
            }

            var token = GetToken(user);

            Response.Headers.Add("Authorization", token);

            return Ok(user);
        }


        private string GetToken(User user)
        {
            var claims = new List<Claim>
            {
                new(JwtClaimIdentifiers.Id,  user.Id.ToString())
            };

            var jwt = new JwtSecurityToken(
                issuer: _jwtAuthOptions.Issuer,
                audience: _jwtAuthOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
                signingCredentials: new SigningCredentials(_jwtAuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }
    }
}