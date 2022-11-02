using Microsoft.AspNetCore.Authorization;
using TestWebApiWithJwtAuth.Services;

namespace TestWebApiWithJwtAuth.Authenticate
{
    public class ExtendedAuthorizationHandler : AuthorizationHandler<ExtendedAuthorizationRequirement>
    {
        private readonly IUserService _userService;
        private readonly ILogger<ExtendedAuthorizationHandler> _logger;

        public ExtendedAuthorizationHandler(IUserService userService, ILogger<ExtendedAuthorizationHandler> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ExtendedAuthorizationRequirement requirement)
        {

            try
            {
                var userIdClaim = context.User.Claims.First(i => string.Equals(i.Type, JwtClaimIdentifiers.Id));

                var user = _userService.GetUserById(int.Parse(userIdClaim.Value));

                //var userFunctionUrls = user.Roles.SelectMany(r => r.Functions).Select(f => f.Url).Distinct();

                var httpContext = context.Resource as DefaultHttpContext;

                if (httpContext != null &&  httpContext.Request.Path.HasValue)
                {
                    var requestAction = httpContext.Request.Path.ToString();
                    //var any = userFunctionUrls.Any(url => requestAction.Contains((string)url, StringComparison.OrdinalIgnoreCase));

                    if (true)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            catch (Exception err)
            {
                _logger.LogWarning(err.Message);
            }
        }
    }
}