using System.Collections.Generic;
using NUnit.Framework;
using Pods;

namespace PodsTests.HandUtilTests
{
    public class SetTests : HandUtilTestBase
    {
        [Test]
        public void TestHighCard()
        {
            List<Card> hand = MakeHand(Rank.Ace, Rank.Queen, Rank.Jack, Rank.Ten, Rank.Nine);
            Sets sets = HandUtil.GetSets(hand, out HandType handType, out Rank rank);
            Assert.AreEqual(HandType.HighCard, handType);
            Assert.AreEqual(Rank.Ace, rank);
        }

        [Test]
        public void TestFourOfAKind()
        {
            List<Card> hand = MakeHand(
                Rank.Eight, Rank.Eight, Rank.Eight, Rank.Eight, 
                Rank.Five, Rank.Five, Rank.Five);
            Sets sets = HandUtil.GetSets(hand, out HandType handType, out Rank rank);
            Assert.AreEqual(HandType.FourOfAKind, handType);
            Assert.AreEqual(Rank.Eight, rank);
        }

        [Test]
        public void TestFullHouse()
        {
            List<Card> hand = MakeHand(
                Rank.Ace, Rank.Ace, Rank.Ace, 
                Rank.Eight, Rank.Eight, Rank.Eight, 
                Rank.Two, Rank.Two);
            
            Sets sets = HandUtil.GetSets(hand, out HandType handType, out Rank rank);
            var expectedSets = new Sets(new List<Set>(new[]
            {
                new Set(Rank.Ace, 3),
                new Set(Rank.Eight, 2)
            }));
            
            Assert.AreEqual(expectedSets, sets);
            Assert.AreEqual(HandType.FullHouse, handType);
            Assert.AreEqual(Rank.Ace, rank);
        }
    }
}