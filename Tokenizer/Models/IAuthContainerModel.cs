using System.Security.Claims;

namespace Tokenizer.Models
{
    public interface IAuthJWTContainerModel
    {
        string SecretKey { get; set; }
        string SecurityAlgorithm { get; set; }
        int ExpirationInMinutes { get; set; }
        Claim[] ArrayOfClaims { get; set; }
    }
}
