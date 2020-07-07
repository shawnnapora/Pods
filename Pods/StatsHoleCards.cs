using System;

namespace Pods
{
    // it's kind of like a player, but immutable and thoroughly concerned about the _stats_ of cards vs.
    // being able to calculate actual hands against community cards later.
    public class StatsHoleCards
    {
        public readonly Rank Rank1;
        public readonly Rank Rank2;
        public readonly bool Suited;

        public StatsHoleCards(Player player)
        {
            if (player.Card1 is null || player.Card2 is null)
            {
                throw new ArgumentException("Cannot generate from player without cards");
            }

            // sort the ranks so that equality is easy/the same for e.g. KA and AK
            if (player.Card1.Rank >= player.Card2.Rank)
            {
                Rank1 = player.Card1.Rank;
                Rank2 = player.Card2.Rank;
            }
            else
            {
                Rank1 = player.Card2.Rank;
                Rank2 = player.Card1.Rank;
            }
            
            Suited = player.Card1.Suit == player.Card2.Suit;
        }
        
        public override bool Equals(object? obj)
        {
            return obj is StatsHoleCards other && Rank1 == other.Rank1 && Rank2 == other.Rank2 && Suited == other.Suited;
        }
        
        public override int GetHashCode() => Rank1.GetHashCode() ^ Rank2.GetHashCode() ^ Suited.GetHashCode();
        
        public override string ToString() => $"{Rank1.Stringify()}{Rank2.Stringify()}{(Suited ? "s" : "o")}";

    }
}