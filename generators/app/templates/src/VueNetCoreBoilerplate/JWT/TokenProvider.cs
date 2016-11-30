using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using <%= name %>.Global.Options;

namespace <%= name %>.JWT {
    public class TokenProvider {
        private readonly JwtOptions _appOptions;
        private readonly TokenProviderOptions _options;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public TokenProvider(IOptions<JwtOptions> appOptions) {
            _appOptions = appOptions.Value;

            var signingKey = SigningKey();

            _options = new TokenProviderOptions {
                Audience = _appOptions.JWT_AUDIENCE,
                Issuer = _appOptions.JWT_ISSUER,
                Expiration = _appOptions.JWT_EXPIRATION,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };
        }

        private SymmetricSecurityKey SigningKey() {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appOptions.JWT_SECRET));
        }

        public Task<object> GenerateTokenAsync(ClaimsPrincipal principal) {
            if (principal?.Identity == null) {
                throw new HttpRequestException("Invalid username or password.");
            }

            var now = DateTime.UtcNow;

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = principal.Claims.ToList();
            var jwtClaims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, principal.Identity.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            foreach (var claim in jwtClaims) {
                if (claims.Any(x => x.Type == claim.Type))
                    claims.RemoveAll(x => x.Type == claim.Type);
                claims.Add(claim);
            }

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims.ToArray(),
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new {
                access_token = encodedJwt,
                expires_in = (int) _options.Expiration.TotalSeconds
            };

            return Task.FromResult((object)response);
        }

        public JwtBearerOptions BuildBearerOptions() {
            var tokenValidationParameters = new TokenValidationParameters {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SigningKey(),

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = _appOptions.JWT_ISSUER,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = _appOptions.JWT_AUDIENCE,

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            return new JwtBearerOptions {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            };
        }
    }
}