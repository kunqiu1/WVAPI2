using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IBApi;
using WVAPIDataModels;
namespace WVIB
{
    public partial class IBCore
    {
        private void _core_OnManagedAccounts(object sender, ManagedAccountsArg e)
        {
            _AccountName = e.AccountName;
            _Connected = true;
            _Client.reqAccountUpdates(true, _AccountName);
            _Client.reqPnL(17000, _AccountName, "");
            _Client.reqMarketDataType(2);
        }
        private void _core_OnUpdatePortfolio(object sender, UpdatePortfolioArg e)
        {
            IBPortfolioModel pot = new IBPortfolioModel(e.contract, e.position, e.marketPrice, e.marketValue, e.averageCost, e.unrealizedPNL, e.realizedPNL, e.accountName);
            if (!_Portfolios.Any(a => a.tickerName == pot.contract.LocalSymbol))
                _Portfolios.Add(pot);
            else
            {
                _Portfolios.RemoveAll(x => x.tickerName == pot.contract.LocalSymbol);
                _Portfolios.Add(pot);
            }
            if (_DailyContractPL.All(x => x.Value.ConId != pot.contract.ConId))
            {
                int reqid = 1;
                if (_DailyContractPL.Count() > 0)
                {
                    reqid = _DailyContractPL.Keys.ToList().Max() + 1;
                }
                _DailyContractPL.Add(reqid, pot.contract);
                _Client.reqPnLSingle(reqid, _AccountName, "", pot.contractID);
            }
            foreach (IBPortfolioModel _port in _Portfolios)
                _port.LastUpdate = DateTime.Now;
        }
        private void _Core_OnUpdateAccount(object sender, UpdateAccountArg e)
        {
            IBAccountModel acc = new IBAccountModel(e.key, e.value, e.currency, e.accountName);
            if (e.key == "TotalCashValue")
            {
                UpdatePortfolioArg temp = new UpdatePortfolioArg(Convert.ToDouble(e.value), e.accountName);
                _core_OnUpdatePortfolio(null, temp);
            }
            if (!_Account.Any(a => a.key == acc.key))
                _Account.Add(acc);
            else
            {
                _Account.RemoveAll(x => x.key == acc.key);
                _Account.Add(acc);
            }
            foreach (IBAccountModel _acc in _Account)
                _acc.LastUpdate = DateTime.Now;
        }
        private void _Core_OnError1(object sender, ErrorMessageArg e)
        {
        }

        private void _Core_OnManagedTickPrice(object sender, TickPriceArg e)
        {
            string field = TickType.getField(e.field);
            var mkt = _MktData.Where(x => x.ReqId == e.tickerId).FirstOrDefault();
            if (mkt == null)
            {
                mkt = new MktData(e.tickerId, field, e.price);
                _MktData.Add(mkt);
            }
            else
            {
                mkt.AddUpdate(field, e.price);
            }
        }
        private void _Core_OntickOptionComputation(object sender, TickOptionComputationArg e)
        {
            var mkt = _MktData.Where(x => x.ReqId == e.tickerId).FirstOrDefault();
            if (mkt == null)
            {
                mkt = new MktData(e.tickerId, "delta", e.delta);
                _MktData.Add(mkt);
            }
            else
            {
                mkt.AddUpdate("delta", e.delta);
            }
            mkt.AddUpdate("gamma", e.gamma);
            mkt.AddUpdate("theta", e.theta);
            mkt.AddUpdate("vega", e.vega);
            mkt.AddUpdate("iv", e.impliedVolatility);
            mkt.AddUpdate("optPrice", e.optPrice);
            mkt.AddUpdate("undPrice", e.undPrice);
            mkt.AddUpdate("pvDividend", e.pvDividend);
        }
        private void _Core_OnpnlSingle(object sender, PnlSingleArg e)
        {
            if (e.dailyPnL != double.MaxValue)
            {
                if (e.dailyPnL == 0)
                {

                }
                int id = _DailyContractPL.Where(x => x.Key == e.reqId).First().Value.ConId;
                _Portfolios.Where(x => x.contractID == id).First().DailyPNL = e.dailyPnL;
            }
        }
        private void _Core_Onpnl(object sender, PnlArg e)
        {
            if (e.dailyPnL != double.MaxValue)
                _DailyPL = e.dailyPnL;
        }
    }
}
