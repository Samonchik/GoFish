using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish
{
    public class Player
    {
        public static Random Random = new Random();
        private List<Card> hand = new List<Card>();
        private List<Values> books = new List<Values>();
        public IEnumerable<Card> Hand => hand;
        public IEnumerable<Values> Books => books;
        public readonly string Name;
        public static string S(int s) => s == 1 ? "" : "s";
        public string Status => 
            $"{Name} has {hand.Count} card{S(hand.Count)} and {books.Count} book{S(books.Count)}";
        public Player(string name) => Name = name;
        public Player(string name, IEnumerable<Card> cards)
        {
            Name = name;
            hand.AddRange(cards);
        }
        public void GetNextHand(Deck stock)
        {
         while (stock.Count() > 0 && hand.Count < 5)
            {
                hand.Add(stock.Deal(0));
            }
        }
        public IEnumerable<Card> DoYouHaveAny(Values value, Deck deck)
        {
            var matchingCard = hand.Where(card => card.Value == value).OrderBy(card => card.Suit);
            hand = hand.Where(card => card.Value != value).ToList();
            if (hand.Count() == 0) GetNextHand(deck);
            return matchingCard;
        }
        public void AddCardsAndPullOutBooks(IEnumerable<Card> cards)
        {
            hand.AddRange(cards);
            var foundBooks = hand
                .GroupBy(card => card.Value)
                .Where(x => x.Count() == 4)
                .Select(group => group.Key);

            books.AddRange(foundBooks);
            books.Sort();
            hand = hand
                .Where(card => !books.Contains(card.Value))
                .ToList();
        }
        public void DrawCard(Deck stock)
        {
            if (stock.Count > 0)
                AddCardsAndPullOutBooks(new List<Card>() { stock.Deal(0) });
        } 
        public Values RandomValueFromHand() =>
            hand.OrderBy(card => card.Value).Select(card => card).Skip(Random.Next(hand.Count)).First().Value;
        public override string ToString() => Name;
    }
}
