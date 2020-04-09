using System;
using System.Security.Claims;
using Tokenizer.Models;
using Tokenizer.Services;

namespace Tokenizer
{
    public class ConsoleManager
    {
        private readonly IAuthJWTService _authJWTService;
        private readonly IAuthJWTContainerModel _authJWTContainerModel;

        public ConsoleManager(IAuthJWTService authJWTService, IAuthJWTContainerModel authJWTContainerModel)
        {
            _authJWTService = authJWTService;
            _authJWTContainerModel = authJWTContainerModel;
        }

        public void RunProgramWhileEscapeKeyIsNotPressed()
        {
            do
            {
                RunConsoleToAskForClaims();
                Console.Clear();
                Console.WriteLine("Press Escape to leave or any other key to run the program again");
            }
            while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        private void RunConsoleToAskForClaims()
        {
            PopulateArrayOfClaimsByConsoleInput();
            string generatedJWTToken = _authJWTService.GenerateToken();
            WriteTokenOnConsole(generatedJWTToken);
        }

        private void PopulateArrayOfClaimsByConsoleInput() =>
            _authJWTContainerModel.ArrayOfClaims = GenerateArrayOfClaimsFromInput();

        public Claim[] GenerateArrayOfClaimsFromInput()
        {
            Console.Write("How many values do you want in your payload (numbers only) ? ");
            int arrayOfClaimsSize = int.Parse(Console.ReadLine());

            Claim[] arrayOfClaims = new Claim[arrayOfClaimsSize];

            PushValuesInArrayOfClaims(arrayOfClaims);

            return arrayOfClaims;
        }

        private void PushValuesInArrayOfClaims(Claim[] arrayOfClaims)
        {
            for (int i = 0; i < arrayOfClaims.Length; i++)
            {
                Console.Write($"Insert the number {i + 1} value you want to generate a JWT Token with: ");
                arrayOfClaims[i] = new Claim(ClaimTypes.Name, Console.ReadLine());
            }
        }

        private void WriteTokenOnConsole(string token)
        {
            Console.Clear();
            Console.WriteLine("Your token was successfully generated.");
            Console.WriteLine();
            Console.WriteLine(token);
            Console.ReadKey();
        }
    }
}
