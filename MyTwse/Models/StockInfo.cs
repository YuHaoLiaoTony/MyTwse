using System;
using System.Collections.Generic;

namespace MyTwse.Models
{
    public partial class StockInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string YieldRate { get; set; }
        public int DividendYear { get; set; }
        public decimal? PE { get; set; }
        public decimal? PB { get; set; }
        public string FinancialReport { get; set; }
        public DateTime Date { get; set; }
    }
}
