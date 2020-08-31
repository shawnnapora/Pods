using Pods;

namespace PodsWeb.Data
{
    public class SelectCard : Card
    {
        public SelectCard(Card card) : base(card.Suit, card.Rank)
        {
        }

        public bool IsActive;
    }
}