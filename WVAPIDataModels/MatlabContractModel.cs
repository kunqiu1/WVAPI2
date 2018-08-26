using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVAPIDataModels
{
    public class MatlabContractModel
    {

        public int ReqId { get; set; }
        public string TradingClass { get; set; }
        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public string Currency { get; set; }
        public Boolean ReqStatus { get; set; }
        public string ExpDate { get; set; }
        public string SecType { get; set; }
        public int ConId { get; set; }

        public double last { get; set; }
        public double bid { get; set; }
        public double ask { get; set; }
        public double delta { get; set; }
        public double gamma { get; set; }
        public double theta { get; set; }
        public double vega { get; set; }
        public double iv { get; set; }
        public List<double> dailyprice { get; set; }
        public List<double>bar1 { get; set; }
        public List<double> bar2 { get; set; }


    }
}
