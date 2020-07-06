using System;
using System.Collections.Generic;
using System.Linq;

namespace Pods
{
    public class Hand : IComparable
    {
        public HandType HandType { get; }
        public Rank Rank { get; }
        private readonly Sets? _sets; // for deeper tie-breaking of HandTypes of the same set rank
        
        public Hand(List<Card> cards)
        {
            if (cards.Count < 5 || cards.Count > 7)
            {
                throw new ArgumentException("Hands may only be created from five to seven cards");
            }

            // check hand types in descending order of strength, and stop on find:
            
            cards.Sort();
            
            // a royal flush is just a fancy straight flush, so we can just check for straight(flush?)s
            if (HandUtil.TryGetStraightOrStraightFlush(cards, out HandType straightHandType, out Rank straightRank))
            {
                HandType = straightHandType;
                Rank = straightRank;
                
                if (straightHandType == HandType.StraightFlush)
                {
                    return;
                }
            }

            if (HandUtil.TryGetFlush(cards, out Rank? flushRank) && HandType.Flush > HandType)
            {
                HandType = HandType.Flush;
                Rank = flushRank!.Value;
            }

            // all remaining hand types are just sets, tiebroken by descending size then rank ...
            var sets = HandUtil.GetSets(cards, out HandType setHandType, out Rank setRank);
            if (setHandType >= HandType)
            {
                _sets = sets;
                HandType = setHandType;
                Rank = setRank;
            }
        }
        
        public Hand(Player player, Table table) : this(ExtractCards(player, table)) {}

        private static List<Card> ExtractCards(Player player, Table table)
        {
            List<Card> cards = new List<Card>();
            if (player.Card1 != null && player.Card2 != null)
            {
                cards.Add(player.Card1);
                cards.Add(player.Card2);
            }

            cards.AddRange(table.CommunityCards);
            return cards;
        }

        public int CompareTo(object? obj)
        {
            if (!(obj is Hand other))
            {
                throw new ArgumentException("Comparison of hand with non-hand");
            }

            int handComparison = HandType.CompareTo(other.HandType);
            if (handComparison != 0)
            {
                return handComparison;
            }

            int rankComparison = Rank.CompareTo(other.Rank);
            if (rankComparison != 0)
            {
                return rankComparison;
            }

            if (_sets == null)
            {
                return 0;
            }

            // if _sets isn't null under these conditions, other._sets must also be non-null
            return _sets.CompareTo(other._sets!);
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Hand other))
            {
                return false;
            }
            
            if (HandType == other.HandType && Rank == other.Rank)
            {
                if (_sets == null)
                {
                    return true;
                }
                
                return _sets.Equals(other._sets);
            }

            return false;
        }

        public override int GetHashCode() => HandType.GetHashCode() ^ Rank.GetHashCode() ^ (_sets?.GetHashCode() ?? 0);
    }
    
    public sealed class Set : IComparable<Set>
    {
        public Rank Rank { get; }
        public int Size { get; }

        internal Set(Rank rank, int size)
        {
            Rank = rank;
            Size = size;
        }

        public int CompareTo(Set other)
        {
            if (Size == other.Size)
            {
                return Rank.CompareTo(other.Rank);
            }

            return Size.CompareTo(other.Size);
        }

        public override bool Equals(object? obj)
        {
            return obj is Set other && other.Size.Equals(Size) && other.Rank.Equals(Rank);
        }

        public override int GetHashCode() => Size.GetHashCode() ^ Rank.GetHashCode();

        public override string ToString() => $"{Rank} x{Size}";
    }

    public sealed class Sets : IComparable
    {
        private readonly List<Set> _sets;

        internal Sets(List<Set> sets)
        {
            sets.Sort();
            sets.Reverse();
            _sets = PruneSets(sets);
        }

        internal Sets(params Set[] setsArray) : this(new List<Set>(setsArray)) {}

        public Set this[int index] => _sets[index];

        public int Count => _sets.Count;

        public int CompareTo(object? obj)
        {
            if (!(obj is Sets other))
            {
                throw new ArgumentException("Comparison of sets with non-sets");
            }
            
            // interestingly, if one is bigger than the other it's always worse
            int comparison = _sets.Count.CompareTo(other._sets.Count);
            if (comparison != 0)
            {
                return -comparison;
            }

            for (int i = 0; i < _sets.Count; i++)
            {
                comparison = _sets[i].CompareTo(other._sets[i]);
                if (comparison != 0)
                {
                    return comparison;
                }
            }

            return 0;
        }
        
        public override bool Equals(object? obj)
        {
            return obj is Sets other && _sets.SequenceEqual(other._sets);
        }

        // this could be cached if it's used frequently, currently implemented for tests.
        public override int GetHashCode() => _sets.Aggregate(131, (x, s) => s.GetHashCode() ^ x);

        public override string ToString() => $"[{String.Join(", ", _sets)}]";

        private static List<Set> PruneSets(List<Set> sets)
        {
            // this operates on a list of sets that is already sorted by size, rank
            List<Set> prunedSets = new List<Set>();
            int cardsNeeded = 5;
            
            for (int i = 0; i < sets.Count; i++)
            {
                if (cardsNeeded == 0)
                {
                    break;
                }

                if (cardsNeeded - sets[i].Size >= 0)
                {
                    prunedSets.Add(sets[i]);
                    cardsNeeded -= sets[i].Size;
                    continue;
                }
                
                // we need to search the remaining cards to find the best ones
                var tieBreakerCandidates = sets.Skip(i).ToList();
                prunedSets.AddRange(GetTieBreakers(tieBreakerCandidates, cardsNeeded));
                break;
            }
            
            return prunedSets;
        }
        
        private static List<Set> GetTieBreakers(List<Set> candidates, int needed)
        {
            candidates.Sort((c1, c2) => c2.Rank.CompareTo(c1.Rank));
            List<Set> tieBreakers = new List<Set>();
            int found = 0;
            int i = 0;
            while (found < needed)
            {
                if (candidates[i].Size <= needed)
                {
                    tieBreakers.Add(candidates[i]);
                    needed -= candidates[i].Size;
                    i++;
                    continue;
                }
                
                tieBreakers.Add(new Set(candidates[i].Rank, needed));
                break;
            }

            return tieBreakers;
        }
    }
    
    public enum HandType
    {
        HighCard,
        Pair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush
    }
}