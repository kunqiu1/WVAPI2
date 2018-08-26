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
                if (!_contract.ReqStatus)
                {
                    switch (_contract.SecType)
                    {
                        case "Futures":
                            SubscribeFutures(_contract.Symbol, _contract.ExpDate, _contract.Exchange, _contract.ReqId);
                            break;
                        case "Option":
                            SubscribeOptions(_contract.ConId,_contract.Exchange, _contract.ReqId);
                            break;
                        case "Equity":
                            SubscribeStocks(_contract.ConId, _contract.ReqId);
                            break;
                    }
                }
            }
            _MatContracts.AddRange(mat);
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
