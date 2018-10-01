using IBApi;
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
        public Contract contract { get; set; }
        public double close { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }

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
        public double lastunderlying { get; set; }

        public MatlabContractModel(Contract contrac1,int i)
        {
            this.contract = contrac1;
            ReqId = i;
        }
    }
}
