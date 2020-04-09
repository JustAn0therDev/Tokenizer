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
                IAuthJWTContainerModel authJWTContainerModel = new JWTContainer(GenerateArrayOfClaimsFromInputWithFixedLength());
                IAuthJWTService authJWTService = new JWTService(authJWTContainerModel.SecretKey, authJWTContainerModel);
                string generatedJWTToken = authJWTService.GenerateToken();
                WriteTokenOnConsole(generatedJWTToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message ?? ex.InnerException.Message);
            }
        }

        private static Claim[] GenerateArrayOfClaimsFromInputWithFixedLength()
        {
            Console.Write("How many values do you want in your payload (numbers only) ? ");
            int numberOfValuesInPayload = int.Parse(Console.ReadLine());

            Claim[] arrayOfClaims = new Claim[numberOfValuesInPayload];

            PushValuesInArrayOfClaims(arrayOfClaims);

            return arrayOfClaims;
        }

        private static void PushValuesInArrayOfClaims(Claim[] arrayOfClaims)
        {
            for (int i = 0; i < arrayOfClaims.Length; i++)
            {
                Console.Write($"Insert the number {i + 1} value you want to generate a JWT Token with: ");
                arrayOfClaims[i] = new Claim(ClaimTypes.Name, Console.ReadLine());
            }
        }

        private static void WriteTokenOnConsole(string token)
        {
            Console.WriteLine();
            Console.WriteLine("Your token was successfully generated.");
            Console.WriteLine();
            Console.WriteLine(token);
            Console.ReadKey();
        }
    }
}
