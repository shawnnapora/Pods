using System;
using NUnit.Framework;
using Pods;
using PodsTests.HandUtilTests;

namespace PodsTests
{
    public class HandTests : HandUtilTestBase
    {
        [Test]
        public void TestHandTypeOrder()
        {
            Assert.Greater(HandType.StraightFlush, HandType.Flush);
        }

        [Test]
        public void TestTwoPairTiebreaker()
        {
            var hand1 = new Hand(MakeHand(Rank.Ace, Rank.Ace, Rank.Eight, Rank.Eight, Rank.King));
            var hand2 = new Hand(MakeHand(Rank.Ace, Rank.Ace, Rank.Eight, Rank.Eight, Rank.Queen));
            Assert.AreNotEqual(hand1, hand2);
            Assert.Greater(hand1, hand2);
        }

        [Test]
        public void TestFullHouse()
        {
            var hand1 = new Hand(MakeHand(Rank.Ace, Rank.Ace, Rank.Ace, Rank.Eight, Rank.Eight));
            var hand2 = new Hand(MakeHand(Rank.Ace, Rank.Ace, Rank.Eight, Rank.Eight, Rank.Eight));
            Assert.AreNotEqual(hand1, hand2);
            Assert.Greater(hand1, hand2);
        }

        [Test]
        public void TestInvalidHand()
        {
            var invalidHand = MakeHand(Rank.Ace);
            Assert.Throws<ArgumentException>(() => new Hand(invalidHand));
        }

        [Test]
        public void TestStraight()
        {
            var hand = new Hand(MakeHand(Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six));
            Assert.AreEqual(HandType.Straight, hand.HandType);
            Assert.AreEqual(Rank.Six, hand.Rank);
        }

        [Test]
        public void TestStraightFlush()
        {
            var hand = new Hand(MakeHand(alsoFlush: true, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six));
            Assert.AreEqual(HandType.StraightFlush, hand.HandType);
            Assert.AreEqual(Rank.Six, hand.Rank);
        }

        [Test]
        public void TestFlush()
        {
            var hand = new Hand(MakeHand(Suit.Diamonds, Suit.Diamonds, Suit.Diamonds, Suit.Diamonds, Suit.Diamonds));
            Assert.AreEqual(HandType.Flush, hand.HandType);
        }

        [Test]
        public void TestEqualSetHands()
        {
            var hand1 = new Hand(MakeHand(Rank.Ace, Rank.Ace, Rank.Eight, Rank.Eight, Rank.King));
            var hand2 = new Hand(MakeHand(Rank.Ace, Rank.Ace, Rank.Eight, Rank.Eight, Rank.King));
            Assert.AreEqual(HandType.TwoPair, hand1.HandType);
            Assert.AreEqual(hand1, hand2);
        }

        [Test]
        public void TestUnequalSetsTieBreak()
        {
            var twoPair = new Sets(new Set(Rank.Ace, 2), new Set(Rank.Eight, 2), new Set(Rank.King, 1));
            var fullHouse = new Sets(new Set(Rank.Ace, 3), new Set(Rank.Eight, 2));
            Assert.Greater(fullHouse, twoPair);
        }

        [Test]
        public void TestEqualStraights()
        {
            var hand1 = new Hand(MakeHand(Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Ace));
            var hand2 = new Hand(MakeHand(Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Ace));
            Assert.AreEqual(hand1, hand2);
        }

        [Test]
        public void TestLastHighCardHoldemTiebreaker()
        {
            var hand1 = new Hand(MakeHand(Rank.Ace, Rank.King, Rank.Queen, Rank.Jack, Rank.Seven, Rank.Four, Rank.Three));
            var hand2 = new Hand(MakeHand(Rank.Ace, Rank.King, Rank.Queen, Rank.Jack, Rank.Four, Rank.Three, Rank.Two));
            Assert.AreEqual(HandType.HighCard, hand1.HandType);
            Assert.AreEqual(HandType.HighCard, hand2.HandType);
            Assert.Greater(hand1, hand2);
        }
    }
}