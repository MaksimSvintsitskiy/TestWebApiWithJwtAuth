using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TestWebApiWithJwtAuth.Authenticate
{
    public class JwtAuthOptions
    {
        private const string ISSUER = "TestWebApiWithJwtAuth"; // издатель токена
        private const string AUDIENCE = "MyAuthClient"; // потребитель токена

        public JwtAuthOptions()
        {
            Key = Guid.NewGuid().ToString("N");
        }

        public string Issuer => ISSUER;
        public string Audience => AUDIENCE;
        public string Key { get; } 
        
        public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
    }
}
