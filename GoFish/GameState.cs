using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish
{
    public class GameState
    {
        public readonly IEnumerable<Player> Players;
        public readonly IEnumerable<Player> Opponents;
        public readonly Player HumanPlayer;
        public bool GameOver { get; private set; } = false;
        public readonly Deck Stock;
        public GameState(string humanPlayerName, IEnumerable<string> opponentsName, Deck stock)
        {
            Stock = stock;
            HumanPlayer = new Player(humanPlayerName);
            HumanPlayer.GetNextHand(stock);

            var opponentsList = new List<Player>();

            foreach (var opponentName in opponentsName)
            {
                var opponent = new Player(opponentName);
                opponent.GetNextHand(stock);
                opponentsList.Add(opponent);
            }
            Opponents = opponentsList;
            Players = new List<Player>() { HumanPlayer }.Concat(Opponents);
        }
        public Player RandomPlayer(Player currentPlayer) => Players
            .Where(player => player != currentPlayer)
            .Skip(Player.Random.Next(Players.Count() - 1))
            .First();
        public string PlayRound(Player player, Player playerToAsk, Values valueToAskFor, Deck stock)
        {
            var valuePlural = (valueToAskFor == Values.Six) ? "Sixes" : valueToAskFor + "s";
            var message = new StringBuilder($"{player.Name} asked {playerToAsk.Name} for " +
                $"{valuePlural}{Environment.NewLine}");
            var matchCard = playerToAsk.DoYouHaveAny(valueToAskFor, stock);

            if (matchCard.Any())
            {
                player.AddCardsAndPullOutBooks(matchCard);
                message.Append($"{playerToAsk.Name} has {matchCard.Count()}" +
                    $" {valueToAskFor} card{Player.S(matchCard.Count())}");
            }
            else if (!stock.Any())
            {
                message.Append("The stock is out of card");
            }
            else
            {
                player.DrawCard(stock);
                message.Append($"{player.Name} drew a card");
            }
            if (!player.Hand.Any())
            {
                player.GetNextHand(stock);
                message.Append($"{Environment.NewLine}{player.Name} ran out of cards," +
                    $" drew {player.Hand.Count()} from the stock");
            }
            return message.ToString();
        }
        public string CheckForWinner()
        {
            var playerCards = Players.Select(player => player.Hand.Count()).Sum();
            if (playerCards > 0) return "";
            GameOver = true;
            var winningBookCount = Players.Select(player => player.Books.Count()).Max();
            var winners = Players.Where(player => player.Books.Count() == winningBookCount);
            if (winners.Count() == 1) return $"The winner is {winners.First().Name}";
            return $"The winners are {string.Join(" and ", winners)}";
        }
    }
}
