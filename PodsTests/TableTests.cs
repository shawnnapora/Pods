using System.Linq;
using NUnit.Framework;
using Pods;

namespace PodsTests
{
    public class TableTests
    {
        [Test]
        public void TestTableDeal()
        {
            Table table = new Table(8);
            table.DealAll();
            Assert.AreEqual(8, table.Players.Count);
            Assert.NotNull(table.Players.First().Card1);
            Assert.NotNull(table.Players.Last().Card2);
            Assert.AreEqual(5, table.CommunityCards.Count);
        }
    }
}