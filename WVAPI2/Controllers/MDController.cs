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
        private static IBCore m_ibcore;
        [HttpGet]
        [Route("login/{port}/{id}")]
        public void Login(int port,int id)
        {
            m_ibcore = new IBCore();
            m_ibcore.Login(port, id);

        }

        [HttpGet]
        [Route("logout")]
        public void Logoff()
        {
            if (m_ibcore!=null)
            m_ibcore.Logoff();
        }

        [HttpGet]
        [Route("IsConnected")]
        public Boolean Isconnected() => m_ibcore._Connected;
        [HttpGet]
        [Route("AccountName")]
        public string AccountName() => m_ibcore._AccountName;
        [HttpGet]
        [Route("MarketData")]
        public IEnumerable<MktData> GetMarketData()
        {
            return m_ibcore._MktData;
        }



        [HttpPost]
        [Route("StartMktReq/")]
        public void StartMktReq([FromBody] List<MatlabContractModel> contracts)
        {
            m_ibcore.SubscribeContract(contracts);
        }

    }
}
