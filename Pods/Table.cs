 using System;
 using System.Collections.Generic;
 using System.Collections.ObjectModel;
 using System.Linq;

namespace Pods
{
    public class Table : ICloneable
    {
        private List<Player> _players = new List<Player>();
        private List<Card> _communityCards = new List<Card>();
        private Deck _deck;

        public ReadOnlyCollection<Player> Players => _players.AsReadOnly();
        public ReadOnlyCollection<Card> CommunityCards => _communityCards.AsReadOnly();

        public Table(int players)
        {
            _deck = new Deck();
            for (int player = 0; player < players; player++)
            {
                _players.Add(new Player{OnCardAdded=OnPlayerCard});
            }
        }

        public Table(Player player1, Deck deck, int otherPlayers)
        {
            _deck = deck;
            _players.Add(player1);
            player1.OnCardAdded = OnPlayerCard;
            for (int player = 0; player < otherPlayers; player++)
            {
                _players.Add(new Player{OnCardAdded = OnPlayerCard});
            }
        }

        public void AddCommunityCard(Card card)
        {
            if (_communityCards.Count == 5)
            {
                throw new InvalidOperationException("Cannot add more than 5 cards to community");
            }

            _deck.RemoveCard(card);
            _communityCards.Add(card);
        }

        private void OnPlayerCard(object player, Card card)
        {
            _deck.RemoveCard(card);
        }

        public void DealAll()
        {
            foreach (Player player in _players)
            {
                if (player.Card1 == null)
                {
                    player.Card1 = _deck.Deal();
                }

                if (player.Card2 == null)
                {
                    player.Card2 = _deck.Deal();
                }
            }

            for (int i = 0; i < 5; i++)
            {
                _communityCards.Add(_deck.Deal());
            }
        }

        public override string ToString()
        {
            string playerHands = String.Join(", ", _players.Select((v, i) => $"{i+1}:[{v}]"));
            string communityCards = String.Join(" ", _communityCards);
            return $"Players: {playerHands}, Community: {communityCards}";
        }

        public object Clone()
        {
            return new Table(_players.Count)
            {
                _players = _players.Clone(),
                _communityCards = _communityCards.Clone(),
                _deck = (Deck) _deck.Clone(),
            };
        }
    }
}