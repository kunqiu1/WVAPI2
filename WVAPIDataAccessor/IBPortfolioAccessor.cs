using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVIB;
using WVAPIDataModels;
using WVAPIDbEF;
namespace WVAPIDataAccessor
{
    public static class IBPortfolioAccessor
    {
        public static IEnumerable<IBPortfolioModel> UpdatePortfolio(IEnumerable<IBPortfolioModel> ibport, string accountname)
        {
            RefreshStrategyMapping(ibport, accountname);
            RefreshLongShortRatio(ibport);
            RefreshOptionStrategyMapping(ibport);
            return ibport;
        }
        private static void RefreshOptionStrategyMapping(IEnumerable<IBPortfolioModel> ibport)
        {
            var optport = ibport.Where(x => x.secType == "OPT" || x.secType == "FOP");
            for (int i = 0; i < optport.Count(); i++)
            {
                var opt = new List<IBPortfolioModel>();
                if (optport.ElementAt(i).strategyNameOption == null)
                {
                    opt.Add(optport.ElementAt(i));
                    for (int j = i + 1; j < optport.Count(); j++)
                    {
                        var opt2 = optport.ElementAt(j);

                        if (opt2.contract.Symbol == opt[0].contract.Symbol
                            && opt2.contract.LastTradeDateOrContractMonth == opt[0].contract.LastTradeDateOrContractMonth
                            && Math.Abs(opt2.position) == Math.Abs(opt[0].position))
                        {
                            opt.Add(opt2);
                        }
                    }
                    //Option strategy setup
                    if (opt.Count() == 1)
                    {
                        string dir = opt[0].position > 0 ? "Long" : "Short";
                        opt[0].strategyNameOption = $"{dir} {opt[0].contract.Symbol} " +
                            $"{opt[0].contract.LastTradeDateOrContractMonth}  {opt[0].contract.Right}";
                    }
                    else
                    {
                        opt = opt.OrderBy(x => x.contract.Strike).ToList();
                        string type = "Combo";
                        string dir = opt.Count().ToString();
                        if (opt.Count == 2)
                        {
                            var leg1 = opt[0];
                            var leg2 = opt[1];
                            if (leg1.contract.Right == "C" && leg2.contract.Right == "C")
                            {
                                if (leg1.position * leg2.position < 0)
                                {
                                    if (leg1.contract.LastTradeDateOrContractMonth=="20180921")
                                    {

                                    }
                                    type = "Call Spread";
                                    dir = leg1.position > 0 ? "Long" : "Short";
                                }
                            }
                            if (leg1.contract.Right == "P" && leg2.contract.Right == "P")
                            {
                                if (leg1.position * leg2.position < 0)
                                {
                                    type = "Put Spread";
                                    dir = leg1.position < 0 ? "Long" : "Short";
                                }
                            }
                        }
                        foreach (var item in opt)
                            item.strategyNameOption = $"{dir} {type} {item.contract.Symbol} " +
                       $"{item.contract.LastTradeDateOrContractMonth}";
                    }
                }
            }
        }
        private static void RefreshStrategyMapping(IEnumerable<IBPortfolioModel> ibport, string accountname)
        {
            var _ibstrategymapping = SQLQueryAccessor.GetIbStrategyMapping(accountname);
            var _newmapping = new List<IBStrategyMapping>();
            foreach (IBPortfolioModel port in ibport)
            {
                if (!_ibstrategymapping.Select(x => x.TickerName).Contains(port.tickerName))
                {
                    _newmapping.Add(new IBStrategyMapping() { TickerName = port.tickerName, AccountName = accountname, LastUpdated = DateTime.Now, IBStrategy = "UNKNOWN" });
                }
            }
            if (_newmapping.Count > 0)
            {
                SQLQueryAccessor.InsertIbStrategyMapping(_newmapping);
                _ibstrategymapping = SQLQueryAccessor.GetIbStrategyMapping(accountname);
            }
            foreach (IBPortfolioModel port in ibport)
            {
                port.strategyName = _ibstrategymapping.Where(x => x.TickerName == port.tickerName && x.AccountName == accountname).Select(x => x.IBStrategy).First();
            }
        }
        private static void RefreshLongShortRatio(IEnumerable<IBPortfolioModel> ibport)
        {
            var _longshort = SQLQueryAccessor.GetIbLongShort();
            var _newlongshort = new List<IBLongShortRatio>();
            foreach (IBPortfolioModel port in ibport)
            {
                if (!_longshort.Select(x => x.TickerName).Contains(port.tickerName) && port.strategyName == "LongShort" && port.secType == "STK")
                {
                    _newlongshort.Add(new IBLongShortRatio() { TickerName = port.tickerName, Ratio1 = 1, LastUpdated = DateTime.Now });
                }
            }
            if (_newlongshort.Count > 0)
            {
                SQLQueryAccessor.InsertIbLongShortRatio(_newlongshort);
                _longshort = SQLQueryAccessor.GetIbLongShort();
            }
            foreach (IBPortfolioModel port in ibport)
            {
                if (port.strategyName == "LongShort" && port.secType == "STK")
                    port.longShortRatio = (decimal)_longshort.Where(x => x.TickerName == port.tickerName).Select(x => x.Ratio1).First();
            }
        }

    }
}
