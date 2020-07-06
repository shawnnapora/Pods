using NUnit.Framework;
using Pods;

namespace PodsTests
{
    public class DeckTests
    {
        private Deck Deck { get; set; }

        [SetUp]
        public void Setup()
        {
            Deck = new Deck();
        }
        
        [Test]
        public void TestDeckSize()
        {
            Assert.AreEqual(52, Deck.Size);
        }

        [Test]
        public void TestRemoveCard()
        {
            Deck.RemoveCard(new Card(Suit.Clubs, Rank.Ace));
            Assert.AreEqual(51, Deck.Size);
        }
    }
}