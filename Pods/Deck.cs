using System;
using System.Collections.Generic;
using System.Threading;

namespace Pods
{
    public class Deck : ICloneable
    { 
        private List<Card> _cards;
        public int Size => _cards.Count;
        private bool _shuffled = false;


        public Deck()
        {
            _cards = new List<Card>();
            foreach (Suit suit in _cachedSuits)
            {
                foreach (Rank rank in _cachedRanks)
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
            int n = _cards.Count;  
            while (n > 1) {  
                n--;  
                int k = _random.Value.Next(n + 1);
                var value = _cards[k];  
                _cards[k] = _cards[n];  
                _cards[n] = value;  
            }

            _shuffled = true;
        }
        
        public object Clone()
        {
            return new Deck()
            {
                _cards = _cards.Clone(),
                _shuffled = _shuffled,
            };
        }

        private static readonly Suit[] _cachedSuits = (Suit[])Enum.GetValues(typeof(Suit));
        private static readonly Rank[] _cachedRanks = (Rank[])Enum.GetValues(typeof(Rank));
        private static readonly ThreadLocal<Random> _random = new ThreadLocal<Random>(() => new Random());
    }
}