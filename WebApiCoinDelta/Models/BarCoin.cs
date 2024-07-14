﻿namespace WebApiCoinDelta.Models
{
    public class BarCoin
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        public bool IsSpot { get; set; }
        public int Per { get; set; }
        public DateTime Datetime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Delta_buy { get; set; }
        public double Delta_sell { get; set; }
        public int Index { get; set; }
    }

    public class BarCoinUpdate
    {
        public DateTime Datetime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Delta_buy { get; set; }
        public double Delta_sell { get; set; }
    }
}
