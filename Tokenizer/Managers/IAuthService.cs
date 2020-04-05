using System.Collections.Generic;
using System.Security.Claims;
using Tokenizer.Models;

namespace Tokenizer.Services
{
    public interface IAuthService
    {
        string SecretKey { get; set; }
        IAuthContainerModel AuthContainerModel { get; set; }
        bool IsTokenValid(string token);
        void IsClaimArrayValid();
        string GenerateToken();
        List<Claim> GetTokenClaims(string token);
    }
}
