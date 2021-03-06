﻿using Tokenizer.Models;

namespace Tokenizer.Services
{
    public interface IAuthJWTService
    {
        string SecretKey { get; set; }
        IAuthJWTContainerModel JWTContainerModel { get; set; }
        string GenerateToken();
    }
}
