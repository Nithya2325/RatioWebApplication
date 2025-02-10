using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatioAPIs_Fetching.Models
{
  
        public class RatioRequest
        {
            public DateTime PassingDate { get; set; }
            public List<int> Branches { get; set; }
        }
    
}