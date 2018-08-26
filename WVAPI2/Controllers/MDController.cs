using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IBApi;
using WVAPIDataModels;
using WVIB;
using WVAPIDataAccessor;
using WVAPIDbEF;

namespace WVAPI2.Controllers
{
    [RoutePrefix("api/MD")]
    public class MDController : ApiController
    {
        private static IBCore _ibcore;
        [HttpGet]
        [Route("login/{port}/{id}")]
        public void Login(int port,int id)
        {
            _ibcore = new IBCore();
            _ibcore.Login(port, id);

        }

        [HttpGet]
        [Route("logout")]
        public void Logoff()
        {
            if (_ibcore!=null)
            _ibcore.Logoff();
        }

        [HttpGet]
        [Route("IsConnected")]
        public Boolean Isconnected() => _ibcore._Connected;
        [HttpGet]
        [Route("AccountName")]
        public string AccountName() => _ibcore._AccountName;
        [HttpGet]
        [Route("MarketData")]
        public IEnumerable<MktData> GetMarketData()
        {
            return _ibcore._MktData;
        }



        [HttpPost]
        [Route("StartMktReq/")]
        public void StartMktReq([FromBody] List<MatlabContractModel> contracts)
        {
            _ibcore.SubscribeContract(contracts);
        }

    }
}
