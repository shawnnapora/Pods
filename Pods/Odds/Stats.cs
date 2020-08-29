namespace Pods.Odds
{
    public class Stats
    {
        public int Wins;
        public int Splits;
        public int Losses;

        public int Attempts => Wins + Splits + Losses;
        public double WinRate => ((double) Wins + Splits) / Attempts;
    }
}