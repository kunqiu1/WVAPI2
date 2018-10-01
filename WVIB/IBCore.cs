using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IBApi;
using WVAPIDataModels;
using System.Collections;
namespace WVIB
{
    public partial class IBCore
    {
        public EWrapperImpl _Core;
        public EClientSocket _Client;
        public Boolean _Connected = false;

        public string _AccountName;
        public List<IBPortfolioModel> _Portfolios;
        public List<IBAccountModel> _Account;

        public List<MktData> _MktData;

        public Dictionary<int,Contract> _DailyContractPL;
        public double _DailyPL;

        public IBCore()
        {
            _Core = new EWrapperImpl();
            _Portfolios = new List<IBPortfolioModel>();
            _Account = new List<IBAccountModel>();
            _MktData = new List<MktData>();
            _DailyContractPL = new Dictionary<int, Contract>();

            _Core.OnManagedAccounts += _core_OnManagedAccounts;
            _Core.OnUpdatePortfolio += _core_OnUpdatePortfolio;
            _Core.OnUpdateAccount += _Core_OnUpdateAccount;
            _Core.OnManagedTickPrice += _Core_OnManagedTickPrice;
            _Core.OntickOptionComputation += _Core_OntickOptionComputation;
            _Core.OnpnlSingle += _Core_OnpnlSingle;
            _Core.Onpnl += _Core_Onpnl;
            _Core.OnError1 += _Core_OnError1;


            _Client = _Core.ClientSocket;
        }



        public void Login(int port, int id)
        {
            if (!_Client.IsConnected())
            {
                EReaderSignal readerSignal = _Core.Signal;
                //! [connect]
                _Client.eConnect("127.0.0.1", port, id);
                var reader = new EReader(_Client, readerSignal);
                reader.Start();
                //Once the messages are in the queue, an additional thread can be created to fetch them
                new Thread(() => { while (_Client.IsConnected()) { readerSignal.waitForSignal(); reader.processMsgs(); } }) { IsBackground = true }.Start();
            }
        }



        public void Logoff()
        {
            if (_Client.IsConnected())
            {
                _Client.eDisconnect();
                _Connected = false;
            }
        }





    }
}
