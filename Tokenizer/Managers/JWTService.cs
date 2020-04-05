using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Tokenizer.Models;

namespace Tokenizer.Services
{
    public class JWTService : IAuthService
    {
        public IAuthContainerModel AuthContainerModel { get; set; }
        public string SecretKey { get; set; }

        public JWTService(string secretKey, IAuthContainerModel containerModel)
        {
            SecretKey = secretKey;
            AuthContainerModel = containerModel;
        }

        public string GenerateToken()
        {
            IsClaimArrayValid();

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(AuthContainerModel.Claims),
                Expires = DateTime.UtcNow.AddMinutes(AuthContainerModel.ExpirationInMinutes),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), AuthContainerModel.SecurityAlgorithm)
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;
        }

        public void IsClaimArrayValid()
        {
            if (AuthContainerModel.Claims == null || AuthContainerModel.Claims.Length == 0)
                throw new ArgumentNullException("A list of claims must be provided for a JWT token to be generated.");

            foreach (Claim claim in AuthContainerModel.Claims)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.Email:
                        if (!claim.Value.Contains("@"))
                            throw new ArgumentException("Value is not valid for e-mail claim type.");
                        break;
                    case ClaimTypes.Name:
                        if (string.IsNullOrWhiteSpace(claim.Value))
                            throw new ArgumentException("Value is not valid for e-mail claim type.");
                        break;
                    case ClaimTypes.Role:
                        if (string.IsNullOrWhiteSpace(claim.Value))
                            throw new ArgumentException("Value is not valid for e-mail claim type.");
                        break;
                    default:
                        throw new ArgumentException("ClaimType not supported for verification.");
                }
            }
        }

        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("A token has not been provided");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            jwtSecurityTokenHandler
                .ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return true;
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }

        private SecurityKey GetSymmetricSecurityKey()
        {
            byte[] byteArrayToCreateSymmetricKey = Convert.FromBase64String(SecretKey);
            return new SymmetricSecurityKey(byteArrayToCreateSymmetricKey);
        }

        public List<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("The provided token is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
