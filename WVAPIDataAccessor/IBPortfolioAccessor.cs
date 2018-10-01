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
        public static IEnumerable<IBPortfolioModel> UpdatePortfolio(IBCore _ibcore)
        {

            var ibport = _ibcore._Portfolios;
            var accountname = _ibcore._AccountName;
            var staticdata = SQLQueryAccessor.GetIbStaticData();
            RefreshStrategyMapping(ibport, accountname);
            RefreshOptionStrategyMapping(ibport);
            RefreshStaticData(ibport, staticdata);

            return ibport;
        }

        private static void RefreshStaticData(List<IBPortfolioModel> ibport, IEnumerable<IBStaticData> staticdata)
        {
            foreach (var port in ibport)
            {
                var _staticdata = staticdata.Where(x => x.ConId == port.contractID).ToList();
                if (_staticdata.Count() > 0)
                {
                    //Dividend
                    if (_staticdata.Any(x => x.FieldName == StaticField.EQY_DVD_YLD_IND.ToString()))
                    {
                        port.DividendsYield = Convert.ToDouble(_staticdata
                            .Where(x => x.FieldName == StaticField.EQY_DVD_YLD_IND.ToString())
                            .Select(x => x.Value).FirstOrDefault());
                        var lastDate = _staticdata.Where(x => x.FieldName == StaticField.DVD_EX_DT.ToString()).Select(x => x.Value).FirstOrDefault();
                        if (lastDate != null)
                        {
                            port.DividendsAccrued = port.marketValue * (port.DividendsYield) * (DateTime.Today - Convert.ToDateTime(lastDate)).TotalDays / 365;
                        }
                    }
                    else
                    {
                        port.DividendsYield = 0;
                        port.DividendsAccrued = 0;
                    }
                    // Long Short Ratio
                    if (_staticdata.Any(x => x.FieldName == StaticField.FUND_LEVERAGE_AMOUNT.ToString()))
                    {
                        port.longShortRatio = Convert.ToDouble(_staticdata.Where(x => x.FieldName == StaticField.FUND_LEVERAGE_AMOUNT.ToString()).Select(x => x.Value).FirstOrDefault());
                    }
                    else
                    {
                        port.longShortRatio = 1;
                    }
                    // Duration
                    if (_staticdata.Any(x => x.FieldName == StaticField.YAS_MOD_DUR.ToString()))
                    {
                        port.Duration = Convert.ToDouble(_staticdata.Where(x => x.FieldName == StaticField.YAS_MOD_DUR.ToString()).Select(x => x.Value).FirstOrDefault());
                    }
                    else
                    {
                        port.Duration = 0;
                    }
                }
                else
                {
                    port.DividendsYield = 0;
                    port.DividendsAccrued = 0;
                    port.longShortRatio = 1;
                    port.Duration = 0;
                }
                if (port.secType != "OPT" && port.secType != "FOP")
                {
                    port.Delta = port.marketValue * 0.01 * port.longShortRatio;
                }
            }
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
        //private static void RefreshLongShortRatio(IEnumerable<IBPortfolioModel> ibport)
        //{
        //    var _longshort = SQLQueryAccessor.GetIbLongShort();
        //    var _newlongshort = new List<IBLongShortRatio>();
        //    foreach (IBPortfolioModel port in ibport)
        //    {
        //        if (!_longshort.Select(x => x.TickerName).Contains(port.tickerName) && port.strategyName == "LongShort" && port.secType == "STK")
        //        {
        //            _newlongshort.Add(new IBLongShortRatio() { TickerName = port.tickerName, Ratio1 = 1, LastUpdated = DateTime.Now });
        //        }
        //    }
        //    if (_newlongshort.Count > 0)
        //    {
        //        SQLQueryAccessor.InsertIbLongShortRatio(_newlongshort);
        //        _longshort = SQLQueryAccessor.GetIbLongShort();
        //    }
        //    foreach (IBPortfolioModel port in ibport)
        //    {
        //        if (port.strategyName == "LongShort" && port.secType == "STK")
        //            port.longShortRatio = (decimal)_longshort.Where(x => x.TickerName == port.tickerName).Select(x => x.Ratio1).First();
        //    }
        //}

    }
}
