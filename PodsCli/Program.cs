using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Pods;

namespace PodsCli
{
    class Program
    {
        static void Main(string[] args)
        {
            RunMany(200000);
        }

        static void RunOne()
        {
            var table = new Table(2);
            table.DealAll();
            var p1 = new Hand(table.Players[0], table);
            var p2 = new Hand(table.Players[1], table);
            Console.WriteLine(table);
            switch (p1.CompareTo(p2))
            {
                case 1:
                    Console.WriteLine($"Player 1 wins with {p1.HandType}");
                    break;
                case 0:
                    Console.WriteLine("Split pot");
                    break;
                case -1:
                    Console.WriteLine($"Player 2 wins with {p2.HandType}");
                    break;
            }
        }

        static void RunMany(int count)
        {
            var playerStats = new ConcurrentDictionary<Player, Stats>();
            for (int i = 0; i < count; i++)
            {
                var table = new Table(2);
                table.DealAll();
                var p1 = new Hand(table.Players[0], table);
                var p2 = new Hand(table.Players[1], table);
                var p1Stats = playerStats.GetOrAdd(table.Players[0], _ => new Stats());
                var p2Stats = playerStats.GetOrAdd(table.Players[1], _ => new Stats());

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

            var bestPlayerStats = playerStats.Aggregate(BestWinRate);
            var bestPlayer = bestPlayerStats.Key;
            var bestStats = bestPlayerStats.Value;
            Console.WriteLine($"Best hole cards: {bestPlayer} with {bestStats.Wins} wins out of {bestStats.Attempts} ({bestStats.WinRate:%%}) in {count} rounds");
        }

        private static KeyValuePair<Player, Stats> BestWinRate(
            KeyValuePair<Player, Stats> best,
            KeyValuePair<Player, Stats> current)
        {
            return current.Value.WinRate > best.Value?.WinRate ? current : best;
        }

        private class Stats
        {
            public int Wins;
            public int Pushes;
            public int Losses;

            public int Attempts => Wins + Pushes + Losses;
            public double WinRate => (double) Wins / Attempts;
        }
    }
}