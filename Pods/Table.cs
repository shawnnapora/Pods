 using System;
 using System.Collections.Generic;
 using System.Collections.ObjectModel;
 using System.ComponentModel;
 using System.Linq;
 using System.Text;

namespace Pods
{
    public class Table
    {
        private List<Player> _players = new List<Player>();
        private Card[] _communityCards = new Card[5];
        private Deck _deck;

        public ReadOnlyCollection<Player> Players => _players.AsReadOnly();
        public ReadOnlyCollection<Card> CommunityCards => _communityCards.ToList().AsReadOnly();

        public Table(int players)
        {
            _deck = new Deck();
            for (int player = 0; player < players; player++)
            {
                _players.Add(new Player());
            }
        }

        public Table(Player player1, Deck deck, int otherPlayers)
        {
            _deck = deck;
            _players.Add(player1);
            for (int player = 0; player < otherPlayers; player++)
            {
                _players.Add(new Player());
            }
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
                if (_communityCards[i] == null)
                {
                    _communityCards[i] = _deck.Deal();
                }
            }
        }

        public override string ToString()
        {
            string playerHands = String.Join(", ", _players.Select((v, i) => $"{i+1}:[{v}]"));
            string communityCards = String.Join(" ", (IEnumerable<Card>)_communityCards);
            return $"Players: {playerHands}, Community: {communityCards}";
        }
    }
}