using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVAPIDataModels
{
    public class IBPortfolioModel
    {
        public Contract contract { get; set; }
        public double position { get; set; }
        public double marketPrice { get; set; }
        public double marketValue { get; set; }
        public double averageCost { get; set; }
        public double unrealizedPNL { get; set; }
        public double realizedPNL { get; set; }
        public string accountName { get; set; }
        public string tickerName { get; set; }
        public string secType { get; set; }
        public string exchange { get; set; }
        public string strategyName { get; set; }
        public decimal longShortRatio { get; set; }
        public int contractID { get; set; }
        public DateTime LastUpdate { get; set; }

        public IBPortfolioModel(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealizedPNL, double realizedPNL, string accountName)
        {
            contractID = contract.ConId;
            tickerName = contract.LocalSymbol;
            secType = contract.SecType;
            this.position = position;
            this.marketPrice = marketPrice;
            this.marketValue = marketValue;
            this.averageCost = averageCost;
            this.unrealizedPNL = unrealizedPNL;
            this.realizedPNL = realizedPNL;
            this.accountName = accountName;
            this.contract = contract;
            exchange = contract.PrimaryExch;
        }


    }
}
