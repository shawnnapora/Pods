using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Pods.Odds
{
    public static partial class PreflopOdds
    {
        private static StatsHoleCards Hand(string statString)
        {
            return new StatsHoleCards(Player.FromStatsString(statString));
        }

        public static IDictionary<StatsHoleCards, Stats> RunMany(int count)
        {
            var playerStats = new ConcurrentDictionary<StatsHoleCards, Stats>();
            for (int i = 0; i < count; i++)
            {
                var table = new Table(2);
                table.DealAll();
                AccumulateStats(table, playerStats);
            }

            return playerStats;
        }

        public static IDictionary<StatsHoleCards, Stats> RunMany(Player player, int players, int count)
        {
            var playerStats = new ConcurrentDictionary<StatsHoleCards, Stats>();
            
            for (int i = 0; i < count; i++)
            {
                var deck = new Deck();
                deck.RemoveCard(player.Card1!);
                deck.RemoveCard(player.Card2!);
                var table = new Table(player, deck, players - 1);
                table.DealAll();
                AccumulateStats(table, playerStats);
            }

            return playerStats;
        }

        private static void AccumulateStats(
            Table table, 
            ConcurrentDictionary<StatsHoleCards, Stats> playerStats)
        {
            var hands = table.Players.Select(c => new Hand(c, table)).ToArray();
            var p1Stats = playerStats.GetOrAdd(new StatsHoleCards(table.Players[0]), _ => new Stats());

            var p1Hand = hands[0];
            int worstCompare = 1;
            for (int i = 1; i < hands.Length; i++)
            {
                int currentCompare = p1Hand.CompareTo(hands[i]);
                if (currentCompare < 0)
                {
                    p1Stats.Losses += 1;
                    return;
                }

                if (worstCompare > currentCompare)
                {
                    worstCompare = currentCompare;
                }
            }

            if (worstCompare == 0)
            {
                p1Stats.Splits += 1;
            }
            else
            {
                p1Stats.Wins += 1;
            }
        }
    }
}