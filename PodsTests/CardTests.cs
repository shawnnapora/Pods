using NUnit.Framework;
using Pods;

namespace PodsTests
{
    public class CardTests
    {
        [Test]
        public void TestCardsEqual()
        {
            Assert.AreEqual(new Card(Suit.Clubs, Rank.Ace), new Card(Suit.Clubs, Rank.Ace));
        }
    }
}