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
        }
    }
}
