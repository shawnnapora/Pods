using System;
using System.Collections.Generic;

namespace Pods
{
    public class Deck
    { 
        private List<Card> _cards;
        public int Size => _cards.Count;
        private bool _shuffled = false;


        public Deck()
        {
            _cards = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    _cards.Add(new Card(suit, rank));
                }
            }
        }

        public Card Deal()
        {
            if (Size == 0)
            {
                throw new InvalidOperationException("Can't deal from empty deck");
            }

            if (!_shuffled)
            {
                Shuffle();
            }
            
            var card = _cards[0];
            _cards.RemoveAt(0);
            return card;
        }

        public void RemoveCard(Card card)
        {
            _cards.Remove(card);
        }

        private void Shuffle()
        {
            var random = new Random();  

            int n = _cards.Count;  
            while (n > 1) {  
                n--;  
                int k = random.Next(n + 1);  
                var value = _cards[k];  
                _cards[k] = _cards[n];  
                _cards[n] = value;  
            }

            _shuffled = true;
        }
    }
}