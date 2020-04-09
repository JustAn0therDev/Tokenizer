using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Tokenizer.Models
{
    public class JWTContainer : IAuthJWTContainerModel
    {
        public JWTContainer()
        {
            SecretKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";
            SecurityAlgorithm = SecurityAlgorithms.HmacSha256Signature;
            ExpirationInMinutes = 10080; // One week
        }

        public string SecretKey { get; set; }
        public string SecurityAlgorithm { get; set; }
        public int ExpirationInMinutes { get; set; }
        public Claim[] ArrayOfClaims { get; set; }
    }
}
