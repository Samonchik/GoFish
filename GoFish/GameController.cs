using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish
{
    public class GameController
    {
        private GameState gameState;
        public bool GameOver { get { return gameState.GameOver; } }
        public Player HumanPlayer { get { return gameState.HumanPlayer; } }
        public IEnumerable<Player> Opponents { get { return gameState.Opponents; } }
        public string Status { get; private set; }
        public GameController(string humanPlayerName, IEnumerable<string> computerPlayerName)
        {
            gameState = new GameState(humanPlayerName, computerPlayerName, new Deck().Shuffle());
            Status = $"Starting a new game with players {string.Join(", ", gameState.Players)}";
        }
        public void NextRound(Player playerToAsk, Values valueToAsk)
        {
            Status = gameState.PlayRound(gameState.HumanPlayer, playerToAsk, valueToAsk, gameState.Stock) + Environment.NewLine;
            ComputerPlayersPlayNextRound();
            Status += string.Join (Environment.NewLine,gameState.Players.Select(player=>player.Status));
            Status += $"{Environment.NewLine}The stock has {gameState.Stock.Count()} cards";
            Status += Environment.NewLine + gameState.CheckForWinner();
        }
        private void ComputerPlayersPlayNextRound()
        {
            IEnumerable<Player> computerPlayerWithCard;
            do
            {
                computerPlayerWithCard = gameState.Opponents
                    .Where(player => player.Hand.Any());
                foreach (var player in computerPlayerWithCard)
                {
                    var randomPlayer = gameState.RandomPlayer(player);
                    var randomValue = player.RandomValueFromHand();
                    Status += gameState.PlayRound(player, randomPlayer, randomValue, gameState.Stock) +Environment.NewLine;
                }

            } while (!gameState.HumanPlayer.Hand.Any()&& computerPlayerWithCard.Any());

        }
        public void NewGame()
        {
            string human = HumanPlayer.Name;
            IEnumerable<string> compName = Opponents.Select(Opponents => Opponents.Name);
            var gameStateInstance = new GameState(human, compName, new Deck());
            gameState = gameStateInstance;
            Status = "Starting a new game";
        }
    }
}
