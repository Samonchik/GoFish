using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Threading;

namespace GoFish
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            var humanName = Console.ReadLine();
            Console.Write("Enter the number of computer opponents: ");
            int numberOfOpponents;
            while (!int.TryParse(Console.ReadLine(), out numberOfOpponents) || numberOfOpponents > 4 || numberOfOpponents < 1)
            {
                Console.WriteLine("Please enter a number from 1 to 4");
            }
            Console.WriteLine($"Welcom to the game, {humanName}");
            var nameOfOpponents = new List<string>();

            gameController = new GameController(humanName, Enumerable.Range(1, numberOfOpponents).Select(i => $"Computer #{i}"));
            Console.WriteLine(gameController.Status);
            while (!gameController.GameOver)
            {
                while (!gameController.GameOver)
                {
                    Console.WriteLine("Your hand: ");
                    foreach (var card in gameController.HumanPlayer.Hand.OrderBy(card => card.Suit).OrderBy(card => card.Value))
                    {
                        Console.WriteLine(card);
                    }
                    var valueToAsk = PromptFromAValue();
                    var playerToAsk = PromptForAnOpponent();
                    gameController.NextRound(playerToAsk, valueToAsk);
                    Console.WriteLine(gameController.Status);
                }
                Console.WriteLine("If you want start new game press 'n', or any other key for quit");
                if (Console.ReadKey(true).KeyChar.ToString().ToUpper() == "N") gameController.NewGame();
            }
        }
        static GameController gameController;
        static Values PromptFromAValue()
        {
            var valuesInHnad = gameController.HumanPlayer.Hand.Select(card => card.Value);
            Console.Write("What card value do you whant to ask for? ");
            while (true)
            {
                if (Enum.TryParse(typeof(Values), Console.ReadLine(), out var result) && valuesInHnad.Contains((Values)result))
                {
                    return (Values)result;
                }
                else Console.WriteLine("Please write correct value");
            }
        }
        static Player PromptForAnOpponent()
        {
            var opponentForAsk = gameController.Opponents.ToList();
            for (int i = 1; i <= opponentForAsk.Count; i++)
                Console.WriteLine($"{i}.{opponentForAsk[i - 1]}");
            Console.Write("Who do you whant to ask a card? ");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int numberOfOpponent) && numberOfOpponent <= opponentForAsk.Count && numberOfOpponent > 0)
                    return opponentForAsk[numberOfOpponent - 1];
                else
                    Console.WriteLine("Please enter correct number");
            }
        }

    }
}