using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManager.Models
{
    public class SystemParameter
    {
        public long SystemParameterId { get; set; }
        public string SystemValue { get; set; }
        public string Description { get; set; }
    }
}