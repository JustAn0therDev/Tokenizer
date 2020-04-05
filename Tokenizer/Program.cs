using System;
using System.Security.Claims;
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
                IAuthContainerModel model = new JWTContainer(GenerateArrayOfClaimsFromInput());
                IAuthService authService = new JWTService(model.SecretKey, model);
                string token = authService.GenerateToken();
                WriteTokenOnConsole(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message ?? ex.InnerException.Message);
            }
        }

        static Claim[] GenerateArrayOfClaimsFromInput()
        {
            Console.Write("How many values do you want in your payload (numbers only) ? ");
            int numberOfValuesInPayload = int.Parse(Console.ReadLine());
            Claim[] arrayOfClaims = new Claim[numberOfValuesInPayload];
            for (int i = 0; i < numberOfValuesInPayload; i++)
            {
                Console.Write($"Insert the number {i + 1} value you want to generate a JWT Token with: ");
                arrayOfClaims[i] = new Claim(ClaimTypes.Name, Console.ReadLine());
            }
            return arrayOfClaims;
        }

        static void WriteTokenOnConsole(string token)
        {
            Console.WriteLine();
            Console.WriteLine("Your token was successfully generated.");
            Console.WriteLine();
            Console.WriteLine(token);
            Console.ReadKey();
        }
    }
}
