using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Pods;

namespace PodsCli
{
    class Program
    {
        static void Main(string? hand, int players = 2, int count = 200000)
        {
            if (hand != null)
            {
                RunMany(hand, players, count);
                return;
            }
            
            RunMany(count);
        }

        static void RunMany(int count)
        {
            var playerStats = new ConcurrentDictionary<StatsHoleCards, Stats>();
            for (int i = 0; i < count; i++)
            {
                var table = new Table(2);
                table.DealAll();
                AccumulateStats(table, playerStats);
            }

            var bestPlayerStats = playerStats.Aggregate(BestWinRate);
            var bestPlayer = bestPlayerStats.Key;
            var bestStats = bestPlayerStats.Value;
            Console.WriteLine($"Best hole cards: {bestPlayer} with {bestStats.Wins} wins out of {bestStats.Attempts} ({bestStats.WinRate:P}) in {count} rounds");
        }

        static void RunMany(string hand, int players, int count)
        {
            var player = Player.FromStatsString(hand);
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

            var statsHand = new StatsHoleCards(player);
            Console.WriteLine($"Hand {statsHand} won {playerStats[statsHand].WinRate:P} of the time in {count} rounds with {players} players");
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

        private static KeyValuePair<StatsHoleCards, Stats> BestWinRate(
            KeyValuePair<StatsHoleCards, Stats> best,
            KeyValuePair<StatsHoleCards, Stats> current)
        {
            return current.Value.WinRate > best.Value?.WinRate ? current : best;
        }

        private class Stats
        {
            public int Wins;
            public int Splits;
            public int Losses;

            public int Attempts => Wins + Splits + Losses;
            public double WinRate => ((double) Wins + Splits) / Attempts;
        }
    }
}