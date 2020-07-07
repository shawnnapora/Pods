﻿using System;

namespace Pods
{
    public class Player
    {
        public Card? Card1 { get; set; }
        public Card? Card2 { get; set; }

        public static Player FromStatsString(string hand)
        {
            if (hand.Length > 3 || hand.Length < 2)
            {
                throw new Exception("invalid hand string");
            }

            int card1Index = Array.IndexOf(Card.Ranks, Char.ToUpper(hand[0]));
            int card2Index = Array.IndexOf(Card.Ranks, Char.ToUpper(hand[1]));
            if (card1Index == -1 || card2Index == -1)
            {
                throw new Exception("rank(s) specified were invalid");
            }
            
            if (hand.Length == 3 && hand[2] == 's')
            {
                return new Player()
                {
                    Card1 = new Card(Suit.Clubs, (Rank)card1Index),
                    Card2 = new Card(Suit.Clubs, (Rank)card2Index)
                };
            }

            return new Player()
            {
                Card1 = new Card(Suit.Clubs, (Rank)card1Index),
                Card2 = new Card(Suit.Diamonds, (Rank)card2Index)
            };
        }
        
        public override string ToString() => $"{Card1?.ToString() ?? "??"} {Card2?.ToString() ?? "??"}";
    }
}