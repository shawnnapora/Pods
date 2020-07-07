﻿namespace Pods
{
    public class Player
    {
        public Card? Card1 { get; set; }
        public Card? Card2 { get; set; }

        public override bool Equals(object? obj)
        {
            if (!(obj is Player other))
            {
                return false;
            }

            if (Card1 == null)
            {
                if (other.Card1 != null)
                {
                    return false;
                }
                else if (!Card1!.Equals(other.Card1))
                {
                    return false;
                }
            }
            
            return Card2?.Equals(other.Card2) ?? other.Card2 == null;
        }
        
        // I'm being lazy for now, don't expect to throw non-dealt players into a dictionary and they
        // should be treated as immutable after dealing. I may break this out to an interface later
        // with a different class for immutable use cases.
        public override int GetHashCode() => Card1.GetHashCode() ^ Card2.GetHashCode();

        public override string ToString() => $"{Card1?.ToString() ?? "??"} {Card2?.ToString() ?? "??"}";
    }
}