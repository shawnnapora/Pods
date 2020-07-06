using System.Collections.Generic;
using Pods;

namespace PodsTests.HandUtilTests
{
    public class HandUtilTestBase
    {
        protected List<Card> MakeHand(params Rank[] ranks)
        {
            return MakeHand(false, ranks);
        }
        
        protected List<Card> MakeHand(bool alsoFlush, params Rank[] ranks)
        {
            var cards = new List<Card>();
            int suit = 0;
            foreach (Rank rank in ranks)
            {
                
                cards.Add(new Card((Suit)suit, rank));
                if (!alsoFlush) { suit = ++suit % 4; }
            }

            return cards;
        }
        
        protected List<Card> MakeHand(params Suit[] suits)
        {
            var cards = new List<Card>();
            int rank = 0;
            foreach (Suit suit in suits)
            {
                if (rank % 5 == 0) { rank++; } // let's not make a straight
                cards.Add(new Card(suit, (Rank)rank));
                rank++;
            }

            return cards;
        }
    }
}