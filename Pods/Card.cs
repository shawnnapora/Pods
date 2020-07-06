﻿﻿using System;
 
namespace Pods
{
    public class Card : IComparable<Card>
    {
        public Suit Suit { get; }
        public Rank Rank { get; }

        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public override string ToString() => $"{Rank.Stringify()}{Suit.Stringify()}";

        public override bool Equals(object? obj)
        {
            return obj is Card other && other.Suit.Equals(Suit) && other.Rank.Equals(Rank);
        }

        public override int GetHashCode() => Suit.GetHashCode() ^ Rank.GetHashCode();
        
        public int CompareTo(Card other)
        {
            return Rank.CompareTo(other.Rank);
        }
    }

    public enum Suit
    {
        Clubs,
        Spades,
        Hearts,
        Diamonds
    }

    public enum Rank
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public static class CardExtensions
    {
        public static char Stringify(this Suit suit) => Suits[(int)suit];
        public static char Stringify(this Suit? suit) => suit != null ? Suits[(int)suit] : '?';
        public static char Stringify(this Rank rank) => Ranks[(int)rank];
        public static char Stringify(this Rank? rank) => rank != null ? Ranks[(int)rank] : '?';

        private static readonly char[] Ranks = {'2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'};
        private static readonly char[] Suits = {'♣', '♠', '♥', '♦'};
    }
}