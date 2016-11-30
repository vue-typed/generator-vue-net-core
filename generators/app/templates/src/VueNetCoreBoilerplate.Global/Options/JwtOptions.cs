using System;

namespace <%= name %>.Global.Options {
    public class JwtOptions {
        public TimeSpan JWT_EXPIRATION;
        public string JWT_SECRET { get; set; }
        public string JWT_ISSUER { get; set; }
        public string JWT_AUDIENCE { get; set; }
    }
}