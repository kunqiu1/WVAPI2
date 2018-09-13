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
        public void SubscribeContract(List<MatlabContractModel> mat)
        {
            foreach (MatlabContractModel _contract in mat)
            {
                if (!_MktData.Select(x=>x.ConId).Contains(_contract.contract.ConId))
                {
                    _MktData.Add(new MktData(_contract.ReqId, _contract.contract.ConId));
                    switch (_contract.contract.SecType)
                    {
                        case "FUT":
                            SubscribeFutures(_contract.contract.Symbol, _contract.contract.LastTradeDateOrContractMonth,
                                _contract.contract.PrimaryExch, _contract.ReqId);
                            break;
                        case "FOP":
                            SubscribeOptions(_contract.contract.ConId, _contract.contract.PrimaryExch, _contract.ReqId);
                            break;
                        case "OPT":
                            SubscribeOptions(_contract.contract.ConId,_contract.contract.PrimaryExch, _contract.ReqId);
                            break;
                        case "STK":
                            SubscribeStocks(_contract.contract.ConId, _contract.ReqId);
                            break;
                    }
                }
            }
        }
        public void SubscribeFutures(string symbol, string expdate, string exchange, int id)
        {
            Contract contract = new Contract();
            contract.Symbol = symbol;
            contract.SecType = "FUT";
            contract.Exchange = exchange;
            contract.Currency = "USD";
            contract.LastTradeDateOrContractMonth = expdate;
            _Client.reqMktData(id, contract, "", false, false, null);
        }
        public void SubscribeOptions(int conID, string exchange,int id)
        {
            Contract contract = new Contract();
            contract.ConId = conID;
            contract.Exchange = exchange;
            contract.Currency = "USD";
            _Client.reqMktData(id, contract, "", false, false, null);
        }
        public void SubscribeStocks(int conID, int id)
        {
            Contract contract = new Contract();
            contract.ConId = conID;
            contract.Exchange = "SMART";
            contract.Currency = "USD";
            _Client.reqMktData(id, contract, "", false, false, null);
        }
    }
}
