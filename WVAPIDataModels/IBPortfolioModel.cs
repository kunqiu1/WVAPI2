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
        private double AverageCost;
        public double averageCost
        {
            get
            {
                double result = 0;
                if (secType == "OPT" || secType == "FOP")
                {
                    result = AverageCost;
                }
                else
                if (secType == "FUT")
                {
                    result = AverageCost / Convert.ToDouble(contract.Multiplier);
                }
                else
                {
                    result = AverageCost;
                }
                if (double.IsNaN(result))
                    result = 0;
                return result;
            }
            set
            {
                this.AverageCost = value;
            }
        }
        public double premium { get; set; }
        public double unrealizedPNL { get; set; }
        public double realizedPNL { get; set; }
        public double DailyPNL { get; set; }
        public double Delta { get; set; }
        public double Gamma { get; set; }
        public double Theta { get; set; }
        public double Vega { get; set; }
        public double Underlying { get; set; }
        public string accountName { get; set; }
        public string tickerName { get; set; }
        public string secType { get; set; }
        public string exchange { get; set; }
        public string strategyName { get; set; }
        public string strategyNameOption { get; set; }
        public string Notes { get; set; }
        public double longShortRatio { get; set; }
        public int contractID { get; set; }
        public DateTime LastUpdate { get; set; }
        public double DividendsAccrued { get; set; }
        public double DividendsYield { get; set; }
        public double Duration { get; set; }

        public IBPortfolioModel(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealizedPNL, double realizedPNL, string accountName)
        {
            contractID = contract.ConId;
            tickerName = contract.LocalSymbol;
            secType = contract.SecType;
            this.contract = contract;
            this.position = position;
            this.marketPrice = marketPrice;
            this.marketValue = marketValue;
            this.averageCost = averageCost;
            this.premium = this.averageCost * position;
            this.unrealizedPNL = unrealizedPNL;
            this.realizedPNL = realizedPNL;
            this.accountName = accountName;
            exchange = contract.PrimaryExch;
        }
        public IBPortfolioModel()
        { }

    }
    public enum StaticField
    {
        DVD_EX_DT,
        YAS_MOD_DUR,
        EQY_DVD_YLD_IND,
        FUND_LEVERAGE_AMOUNT,
    };
}
