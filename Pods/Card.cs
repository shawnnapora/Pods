﻿using System;
 
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
            return obj is Card other && other.Rank.Equals(Rank) && other.Suit.Equals(Suit);
        }

        public override int GetHashCode() => Suit.GetHashCode() ^ Rank.GetHashCode();
        
        public int CompareTo(Card other)
        {
            return Rank.CompareTo(other.Rank);
        }
        
        internal static readonly char[] Ranks = {'2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'};
        internal static readonly char[] Suits = {'♣', '♠', '♥', '♦'};
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
        public static char Stringify(this Suit suit) => Card.Suits[(int)suit];
        public static char Stringify(this Suit? suit) => suit != null ? Card.Suits[(int)suit] : '?';
        public static char Stringify(this Rank rank) => Card.Ranks[(int)rank];
        public static char Stringify(this Rank? rank) => rank != null ? Card.Ranks[(int)rank] : '?';


    }
}