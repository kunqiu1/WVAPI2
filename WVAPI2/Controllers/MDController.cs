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

        #region Get API/IB

        [HttpGet]
        [Route("login/{port}/{id}")]
        public void Login(int port, int id)
        {
            m_ibcore = new IBCore();
            m_ibcore.Login(port, id);

        }

        [HttpGet]
        [Route("logout")]
        public void Logoff()
        {
            if (m_ibcore != null)
                m_ibcore.Logoff();
        }

        [HttpGet]
        [Route("IsConnected")]
        public bool Isconnected() => m_ibcore._Client.IsConnected();

        [HttpGet]
        [Route("AccountName")]
        public string AccountName() => m_ibcore._AccountName;

        [HttpGet]
        [Route("MarketData")]
        public IEnumerable<MktData> GetMarketData()
        {
            return m_ibcore._MktData;
        }
        #endregion


        #region Post API/IB
        [HttpPost]
        [Route("StartMktReq/")]
        public void StartMktReq([FromBody] List<MatlabContractModel> contracts)
        {
            m_ibcore.SubscribeContract(contracts);
        }

        [HttpPost]
        [Route("UpdateStaticData")]
        public HttpResponseMessage UpdateStaticData([FromBody] IEnumerable<IBStaticData> value)
        {
            try
            {
                if (!SQLQueryAccessor.UpdateIbStaticData(value))
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "UpdateStaticData Failed");
                return Request.CreateResponse(HttpStatusCode.OK, value);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        #endregion

    }
}
