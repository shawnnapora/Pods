using System;
using System.Collections.Generic;
using System.Linq;

namespace Pods
{
    /// <summary>
    /// This internal class is split out to allow for testing with InternalsVisibleTo.
    /// It is otherwise strongly coupled to the Hand class, and each method requires cards sorted by rank as input.
    /// </summary>
    internal static class HandUtil
    {
        internal static bool TryGetStraightOrStraightFlush(List<Card> cards, out HandType handType, out Rank rank)
        {
            // <disclaimer>
            // this feels horribly imperative but made immediate sense to create. It's encapsulated so
            // can hopefully be easily made less horrible if appropriate later.
            // </disclaimer>
            
            // if our lowest card is a deuce, check for an ace at the end 
            bool checkAceStraight = cards[0].Rank == Rank.Two;
            bool isAlsoFlush = true;
            int straightCount = 1;  // any card is a straight of size one...

            Card lastCard = cards[0];
            for (int i = 0; i < cards.Count; i++)
            {
                Card card = cards[i];
                if (lastCard.Rank == card.Rank - 1)
                {
                    if (lastCard.Suit != card.Suit)
                    {
                        isAlsoFlush = false;
                    }
                    straightCount++;
                    lastCard = card;
                    continue;
                }
                
                if (lastCard.Rank == card.Rank)
                {
                    continue;
                }

                if (straightCount == 4 && checkAceStraight)
                {
                    // we didn't continue a straight, but we needed an ace to complete one.
                    if (cards.Last().Rank == Rank.Ace)
                    {
                        straightCount++;
                        break;
                    }

                    checkAceStraight = false;
                }

                if (straightCount >= 5)
                {
                    break;
                }

                straightCount = 1;
                isAlsoFlush = true;
                lastCard = card;
            }

            if (straightCount >= 5)
            {
                handType = isAlsoFlush ? HandType.StraightFlush : HandType.Straight;
                rank = lastCard.Rank;
                return true;
            }

            handType = HandType.HighCard;
            rank = lastCard.Rank;
            return false;
        }

        internal static bool TryGetFlush(List<Card> cards, out Rank? rank)
        {
            int[] suitCounts = new int[4] {0, 0, 0, 0};
            Rank[] bestRanks = new Rank[4];

            foreach (Card card in cards)
            {
                suitCounts[(int) card.Suit]++;
                bestRanks[(int) card.Suit] = card.Rank;
            }

            for (int i = 0; i < 4; i++)
            {
                if (suitCounts[i] >= 5)
                {
                    rank = bestRanks[i];
                    return true;
                }
            }

            rank = null;
            return false;
        }

        internal static Sets GetSets(List<Card> cards, out HandType handType, out Rank rank)
        {
            Sets sets = GetSets(cards);
            
            // all sets must contain at least two sets (4+1, 3+2, 2+2+1, ...)
            if (sets.Count < 2)
            {
                throw new ArgumentException("GetSets requires at least five cards which should >=2 sets");
            }
            
            switch (sets[0].Size)
            {
                case 4:
                    handType = HandType.FourOfAKind;
                    break;
                case 3:
                    handType = sets[1].Size == 2 ? HandType.FullHouse : HandType.ThreeOfAKind;
                    break;
                case 2:
                    handType = sets[1].Size == 2 ? HandType.TwoPair : HandType.Pair;
                    break;
                default:
                    handType = HandType.HighCard;
                    break;
            }

            rank = sets[0].Rank;
            return sets;
        }

        private static Sets GetSets(List<Card> cards)
        {
            List<Set> sets = new List<Set>();
            int setSize = 1;
            Card lastCard = cards[0];
            for (int i = 1; i < cards.Count; i++)
            {
                if (lastCard?.Rank == cards[i].Rank)
                {
                    setSize++;
                }
                else
                {
                    sets.Add(new Set(lastCard.Rank, setSize));
                    setSize = 1;
                }

                lastCard = cards[i];
            }
            
            sets.Add(new Set(lastCard.Rank, setSize));
            return new Sets(sets);
        }
    }
}