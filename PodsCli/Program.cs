using System;
using System.Collections.Generic;
using System.Linq;
using Pods;
using Pods.Odds;

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

        private static void RunMany(string hand, int players, int count)
        {
            var player = Player.FromStatsString(hand);
            var statsHand = new StatsHoleCards(player);
            var playerStats = PreflopOdds.RunMany(player, players, count);
            Console.WriteLine($"Hand {statsHand} won {playerStats[statsHand].WinRate:P} of the time in {count} rounds with {players} players");
        }

        private static void RunMany(int count)
        {
            var playerStats = PreflopOdds.RunMany(count);
            var bestPlayerStats = playerStats.Aggregate(BestWinRate);
            var bestPlayer = bestPlayerStats.Key;
            var bestStats = bestPlayerStats.Value;
            Console.WriteLine($"Best hole cards: {bestPlayer} with {bestStats.Wins} wins out of {bestStats.Attempts} ({bestStats.WinRate:P}) in {count} rounds");
        }
        
        private static KeyValuePair<StatsHoleCards, Stats> BestWinRate(
            KeyValuePair<StatsHoleCards, Stats> best,
            KeyValuePair<StatsHoleCards, Stats> current)
        {
            return current.Value.WinRate > best.Value?.WinRate ? current : best;
        }
    }
}