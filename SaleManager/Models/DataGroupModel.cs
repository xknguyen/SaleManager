using System.Collections.Generic;

namespace SaleManager.Models
{
    public class DataGroupModel
    {
        public string href { get; set; }
        public string text { get; set; }

        public List<DataGroupModel> nodes { get; set; }

    }
}