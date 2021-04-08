using System;
using System.Collections.Generic;

namespace MyTwse.Models
{
    public partial class InsertDateLog
    {
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
