﻿using System;

namespace Pods
{
    public class Player : ICloneable
    {
        private Card _card1;
        private Card _card2;
        
        public Card? Card1 
        {
            get => _card1;
            set
            {
                if (_card1 != null || value == null)
                {
                    throw new InvalidOperationException();
                }

                _card1 = value;
                OnCardAdded?.Invoke(this, value);
            } 
        }

        public Card? Card2 
        {
            get => _card2;
            set
            {
                if (_card2 != null || value == null)
                {
                    throw new InvalidOperationException();
                }

                _card2 = value;
                OnCardAdded?.Invoke(this, value);
            } 
        }

        public EventHandler<Card> OnCardAdded { get; set; } 

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
                return new Player
                {
                    Card1 = new Card(Suit.Clubs, (Rank)card1Index),
                    Card2 = new Card(Suit.Clubs, (Rank)card2Index)
                };
            }

            return new Player
            {
                Card1 = new Card(Suit.Clubs, (Rank)card1Index),
                Card2 = new Card(Suit.Diamonds, (Rank)card2Index)
            };
        }

        public object Clone()
        {
            return new Player
            {
                _card1 = _card1,
                _card2 = _card2,
            };
        }

        public override string ToString() => $"{Card1?.ToString() ?? "??"} {Card2?.ToString() ?? "??"}";
    }
}