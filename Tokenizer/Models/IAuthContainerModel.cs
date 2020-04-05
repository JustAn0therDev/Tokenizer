using System.Security.Claims;

namespace Tokenizer.Models
{
    public interface IAuthContainerModel
    {
        string SecretKey { get; set; }
        string SecurityAlgorithm { get; set; }
        int ExpirationInMinutes { get; set; }
        Claim[] Claims { get; set; }
    }
}
