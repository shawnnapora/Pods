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
        private List<Card> _communityCards = new List<Card>();
        private Deck _deck = new Deck();

        public ReadOnlyCollection<Player> Players => _players.AsReadOnly();
        public ReadOnlyCollection<Card> CommunityCards => _communityCards.AsReadOnly();

        public Table(int players)
        {
            for (int player = 0; player < players; player++)
            {
                _players.Add(new Player());
            }
        }

        public void DealAll()
        {
            foreach (Player player in _players)
            {
                player.Card1 = _deck.Deal();
                player.Card2 = _deck.Deal();
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
    }
}