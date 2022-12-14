using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Threading;

namespace GoFish
{
    public class Deck : List<Card>
    {
        private static Random random = Player.Random;

        public Deck()
        {
            Reset();
        }
        public void Reset()
        {
            Clear();
            for (int numberOfSuit = 0; numberOfSuit <= (int)Suits.Spades; numberOfSuit++)
            {
                for (int numberOfValue = 1; numberOfValue <= (int)Values.King; numberOfValue++)
                {
                    Add(new Card((Values)numberOfValue, (Suits)numberOfSuit));
                }
            }
        }
        public Card Deal(int index)
        {
            Card cardToDeal = base[index];
            RemoveAt(index);
            return cardToDeal;
        }

        public Deck Shuffle()
        {
            var copyOfDeck = new List<Card>(this);
            Clear();
            while (copyOfDeck.Count > 0)
            {
                int index = random.Next(copyOfDeck.Count);
                var randomCard = copyOfDeck[index];
                Add(randomCard);
                copyOfDeck.RemoveAt(index);
            }
            return this;
        }
    }
}
