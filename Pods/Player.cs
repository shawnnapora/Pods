﻿namespace Pods
{
    public class Player
    {
        public Card? Card1 { get; set; }
        public Card? Card2 { get; set; }
        
        public override string ToString() => $"{Card1?.ToString() ?? "??"} {Card2?.ToString() ?? "??"}";
    }
}