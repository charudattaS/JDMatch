using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDMatch.Application.DTOs
{
    public class UsageInfo
    {
        public int Used { get; set; }
        public int Limit { get; set; }
        public int Remaining { get; set; }
        public DateTime NextBillingDate { get; set; }
    }
}
