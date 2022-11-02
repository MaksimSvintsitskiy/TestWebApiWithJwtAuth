using Microsoft.IdentityModel.Tokens;

namespace TestWebApiWithJwtAuth.Authenticate
{
    public class JwtIssuerOptions
    {
        /// <summary>
        ///     4.1.1.  "iss" (Issuer) Claim - The "iss" (issuer) claim identifies the principal that issued the JWT.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        ///     4.1.2.  "sub" (Subject) Claim - The "sub" (subject) claim identifies the principal that is the subject of the JWT.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///     4.1.3.  "aud" (Audience) Claim - The "aud" (audience) claim identifies the recipients that the JWT is intended for.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        ///     4.1.4.  "exp" (Expiration Time) Claim - The "exp" (expiration time) claim identifies the expiration time on or
        ///     after which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public DateTime Expiration => IssuedAt.Add(JwtTokenTtl);

        /// <summary>
        ///     4.1.5.  "nbf" (Not Before) Claim - The "nbf" (not before) claim identifies the time before which the JWT MUST NOT
        ///     be accepted for processing.
        /// </summary>
        public DateTime NotBefore => DateTime.UtcNow;

        /// <summary>
        ///     4.1.6.  "iat" (Issued At) Claim - The "iat" (issued at) claim identifies the time at which the JWT was issued.
        /// </summary>
        public DateTime IssuedAt => DateTime.UtcNow;

        /// <summary>
        ///     Set the timespan the JWT token will be valid for (default is 20 min)
        /// </summary>
        public TimeSpan JwtTokenTtl { get; set; } = TimeSpan.FromMinutes(3);

        /// <summary>
        ///     Set the timespan the refresh token will be valid for (default is 120 min)
        /// </summary>
        public TimeSpan RefreshTokenTtl { get; set; } = TimeSpan.FromMinutes(120);

        /// <summary>
        ///     "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        public Func<string> JtiGenerator => () => Guid.NewGuid().ToString();

        /// <summary>
        ///     The signing key to use when generating and validating tokens.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        ///     The signing credentials to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        public long IssuedAtUnixFormat => (long)Math.Round((IssuedAt -
                                                            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    }
}
