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
        public string PE { get; set; }
        public string PB { get; set; }
        public string FinancialReport { get; set; }
        public DateTime Date { get; set; }
    }
}
