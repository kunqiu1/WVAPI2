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
    [RoutePrefix("api/IB")]
    public class IBController : ApiController
    {
        private static IBCore _ibcore;

        #region Get API/IB
        [HttpGet]
        [Route("Authen/{username}/{password}")]
        public bool Authen(string username,string password)
        {
            return UserSecurity.Login(username, password);
        }
        [HttpGet]
        [Route("login/{port}/{id}")]
        public void Login(int port, int id)
        {
            _ibcore = new IBCore();
            _ibcore.Login(port, id);
        }

        [HttpGet]
        [Route("logout")]
        public void Logoff()
        {
            if (_ibcore != null)
                _ibcore.Logoff();
        }

        [HttpGet]
        [Route("CashBalanceStart")]
        public decimal CashBalanceStart() => SQLQueryAccessor.GetCashActivity(_ibcore._AccountName);
        [HttpGet]
        [Route("Portfolio")]
        public IEnumerable<IBPortfolioModel> GetPortfolio()
        {
            IBPortfolioAccessor.UpdatePortfolio(_ibcore._Portfolios, _ibcore._AccountName);
            return _ibcore._Portfolios;
        }
        [HttpGet]
        [Route("Account")]
        public IEnumerable<IBAccountModel> GetAccount()
        {
            return _ibcore._Account;
        }
        [HttpGet]
        [Route("Strategy")]
        public IEnumerable<IBStrategy> GetStrategy()
        {
            return SQLQueryAccessor.GetIbStrategy();
        }
        [HttpGet]
        [Route("IsConnected")]
        public Boolean Isconnected()
        {
            if (_ibcore == null)
            {
                return false;
            }
            else
            {
                return _ibcore._Client.IsConnected();
            }
        }

        [HttpGet]
        [Route("AccountName")]
        public string AccountName() => _ibcore._AccountName;

        [HttpGet]
        [Route("DailyPL")]
        public double DailyPL() => _ibcore._DailyPL;

        #endregion


        #region Post API/IB
        [HttpPost]
        [Route("UpdateStrategy")]
        public HttpResponseMessage UpdateStrategy([FromBody] IBStrategyMapping value)
        {
            try
            {
                if (!SQLQueryAccessor.UpdateIbStrategyMapping(value))
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "IBStrategymapping with Accountname: " + value.AccountName + ", TickerName: " + value.TickerName + " not found to update");
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
