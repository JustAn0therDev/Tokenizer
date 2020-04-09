using System;
using Tokenizer.Models;
using Tokenizer.Services;

namespace Tokenizer
{
    class Program
    {
        static void Main()
        {
            try
            {
                IAuthJWTContainerModel authJWTContainerModel = new JWTContainer();
                IAuthJWTService authJWTService = new JWTService(authJWTContainerModel.SecretKey, authJWTContainerModel);

                ConsoleManager consoleManager = new ConsoleManager(authJWTService, authJWTContainerModel);
                consoleManager.RunProgramWhileEscapeKeyIsNotPressed();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message ?? ex.InnerException.Message);
            }
        }
    }
}
