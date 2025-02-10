using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatioAPIs_Fetching.Models
{
    public class RatioResult
    {
        public string MonthYear { get; set; }
        public decimal CurrentRatio { get; set; }
        public decimal QuickRatio { get; set; }
        public decimal CashRatio { get; set; }
    }
}