using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pods;

namespace PodsTests.HandUtilTests
{
    public class StraightTests : HandUtilTestBase
    {
        [Test]
        public void TestLowStraight()
        {
            var cards = MakeHand(alsoFlush: false, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six);
            bool result = HandUtil.TryGetStraightOrStraightFlush(cards, out HandType handType, out Rank rank);
            Assert.AreEqual(true, result);
            Assert.AreEqual(HandType.Straight,handType);
            Assert.AreEqual(Rank.Six, rank);
        }

        [Test]
        public void TestLowAceStraight()
        {
            var cards = MakeHand(Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Ace);
            bool result = HandUtil.TryGetStraightOrStraightFlush(cards, out HandType handType, out Rank rank);
            Assert.AreEqual(true, result);
            Assert.AreEqual(HandType.Straight, handType);
            Assert.AreEqual(Rank.Five, rank);
        }
        
        [Test]
        public void TestHighAceStraight()
        {
            var cards = MakeHand(Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Ace);
            bool result = HandUtil.TryGetStraightOrStraightFlush(cards, out HandType handType, out Rank rank);
            Assert.AreEqual(true, result);
            Assert.AreEqual(HandType.Straight, handType);
            Assert.AreEqual(Rank.Ace, rank);
        }
        
        [Test]
        public void TestHighStraight()
        {
            var cards = MakeHand(Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King);
            bool result = HandUtil.TryGetStraightOrStraightFlush(cards, out HandType handType, out Rank rank);
            Assert.AreEqual(true, result);
            Assert.AreEqual(HandType.Straight, handType);
            Assert.AreEqual(Rank.King, rank);
        }

        [Test]
        public void TestStraightFlush()
        {
            var cards = MakeHand(alsoFlush:true, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King);
            bool result = HandUtil.TryGetStraightOrStraightFlush(cards, out HandType handType, out Rank rank);
            Assert.AreEqual(true, result);
            Assert.AreEqual(HandType.StraightFlush, handType);
            Assert.AreEqual(Rank.King, rank);
        }
        
        [Test]
        public void TestInterruptedHoldemStraight()
        {
            var cards = MakeHand(Rank.Two, Rank.Three, Rank.Five, Rank.Six, Rank.Seven, Rank.Eight, Rank.Nine);
            bool result = HandUtil.TryGetStraightOrStraightFlush(cards, out HandType handType, out Rank rank);
            Assert.AreEqual(true, result);
            Assert.AreEqual(HandType.Straight, handType);
            Assert.AreEqual(Rank.Nine, rank);
        }
        
        [Test]
        public void TestNoStraight()
        {
            var cards = MakeHand(Rank.Eight, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King);
            bool result = HandUtil.TryGetStraightOrStraightFlush(cards, out HandType handType, out Rank rank);
            Assert.AreEqual(false, result);
        }
    }
}