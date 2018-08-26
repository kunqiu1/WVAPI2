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
            return ibport;
        }

        private static void RefreshStrategyMapping(IEnumerable<IBPortfolioModel> ibport, string accountname)
        {
            var _ibstrategymapping = SQLQueryAccessor.GetIbStrategyMapping(accountname);
            var _newmapping = new List<IBStrategyMapping>();
            foreach (IBPortfolioModel port in ibport)
            {
                if (!_ibstrategymapping.Select(x => x.TickerName).Contains(port.tickerName))
                {
                    _newmapping.Add(new IBStrategyMapping() { TickerName = port.tickerName, AccountName = accountname,LastUpdated=DateTime.Now,IBStrategy="UNKNOWN" });
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
        private static void RefreshLongShortRatio (IEnumerable<IBPortfolioModel> ibport)
        {
            var _longshort = SQLQueryAccessor.GetIbLongShort();
            var _newlongshort = new List<IBLongShortRatio>();
            foreach (IBPortfolioModel port in ibport)
            {
                if (!_longshort.Select(x => x.TickerName).Contains(port.tickerName) && port.strategyName=="LongShort" && port.secType=="STK")
                {
                    _newlongshort.Add(new IBLongShortRatio() { TickerName = port.tickerName, Ratio1 = 1, LastUpdated=DateTime.Now });
                }
            }
            if (_newlongshort.Count > 0)
            {
                SQLQueryAccessor.InsertIbLongShortRatio(_newlongshort);
                _longshort = SQLQueryAccessor.GetIbLongShort();
            }
            foreach (IBPortfolioModel port in ibport)
            {
                if (port.strategyName=="LongShort" && port.secType=="STK")
                port.longShortRatio =(decimal) _longshort.Where(x => x.TickerName == port.tickerName ).Select(x => x.Ratio1).First();
            }
        }

    }
}
