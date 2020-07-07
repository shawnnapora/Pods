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
            var p1 = new Hand(table.Players[0], table);
            var p2 = new Hand(table.Players[1], table);
            var p1Stats = playerStats.GetOrAdd(new StatsHoleCards(table.Players[0]), _ => new Stats());
            var p2Stats = playerStats.GetOrAdd(new StatsHoleCards(table.Players[1]), _ => new Stats());

            switch (p1.CompareTo(p2))
            {
                case 1:
                    p1Stats.Wins += 1;
                    p2Stats.Losses += 1;
                    break;
                case 0:
                    p1Stats.Pushes += 1;
                    p2Stats.Pushes += 1;
                    break;
                case -1:
                    p1Stats.Losses += 1;
                    p2Stats.Wins += 1;
                    break;
                default:
                    throw new Exception("invalid result");
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
            public int Pushes;
            public int Losses;

            public int Attempts => Wins + Pushes + Losses;
            public double WinRate => ((double) Wins + Pushes) / Attempts;
        }
    }
}