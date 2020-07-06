using System.Collections.Generic;
using NUnit.Framework;
using Pods;

namespace PodsTests.HandUtilTests
{
    public class FlushTests : HandUtilTestBase
    {
        [Test]
        public void TestFlush()
        {
            var cards = MakeHand(Suit.Clubs, Suit.Clubs, Suit.Clubs, Suit.Clubs, Suit.Clubs);
            bool result = HandUtil.TryGetFlush(cards, out Rank? rank);
            Assert.AreEqual(true, result);
        }
        
        [Test]
        public void TestNotFlush()
        {
            var cards = MakeHand(Suit.Hearts, Suit.Clubs, Suit.Clubs, Suit.Clubs, Suit.Clubs);
            bool result = HandUtil.TryGetFlush(cards, out Rank? rank);
            Assert.AreEqual(false, result);
        }
    }
}