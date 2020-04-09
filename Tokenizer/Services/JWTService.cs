using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Tokenizer.Models;

namespace Tokenizer.Services
{
    public class JWTService : IAuthJWTService
    {
        #region Private Properties

        private JwtSecurityTokenHandler JwtSecurityTokenHandler { get; set; } = new JwtSecurityTokenHandler();
        private SecurityTokenDescriptor SecurityTokenDescriptor
        {
            get
            {
                return new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(JWTContainerModel.ArrayOfClaims),
                    Expires = DateTime.UtcNow.AddMinutes(JWTContainerModel.ExpirationInMinutes),
                    SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), JWTContainerModel.SecurityAlgorithm)
                };
            }
        }

        #endregion

        #region Public Properties

        public IAuthJWTContainerModel JWTContainerModel { get; set; }
        public string SecretKey { get; set; }

        #endregion

        public JWTService(string secretKey, IAuthJWTContainerModel jwtContainerModel)
        {
            SecretKey = secretKey;
            JWTContainerModel = jwtContainerModel;
        }

        private SecurityKey GetSymmetricSecurityKey()
        {
            byte[] byteArrayToCreateSymmetricKey = Convert.FromBase64String(SecretKey);
            return new SymmetricSecurityKey(byteArrayToCreateSymmetricKey);
        }

        public string GenerateToken()
        {
            IsClaimArrayValid();
            SecurityToken securityToken = JwtSecurityTokenHandler.CreateToken(SecurityTokenDescriptor);
            return JwtSecurityTokenHandler.WriteToken(securityToken);
        }

        public void IsClaimArrayValid()
        {
            if (JWTContainerModel.ArrayOfClaims == null || JWTContainerModel.ArrayOfClaims.Length == 0)
                throw new ArgumentNullException("A list of claims must be provided for a JWT token to be generated.");
         
            CheckAllClaimsInClaimArray();
        }

        private void CheckAllClaimsInClaimArray()
        {
            foreach (Claim claim in JWTContainerModel.ArrayOfClaims)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.Email:
                        if (!claim.Value.Contains("@"))
                            throw new ArgumentException("Value is not valid for e-mail claim type.");
                        break;
                    case ClaimTypes.Name:
                        if (string.IsNullOrWhiteSpace(claim.Value))
                            throw new ArgumentException("Empty value is not valid for this claim type.");
                        break;
                    case ClaimTypes.Role:
                        if (claim.Value.IndexOf(" ") > -1)
                            throw new ArgumentException("Space separated value is not valid for role claim type.");
                        break;
                    default:
                        throw new ArgumentException("ClaimType not supported for verification.");
                }
            }
        }
    }
}
