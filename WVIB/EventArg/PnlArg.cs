using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVIB
{
    public class PnlArg
    {
        public int reqId;
        public double dailyPnL;
        public double unrealizedPnL;
        public double realizedPnL;

        public PnlArg(int reqId, double dailyPnL, double unrealizedPnL, double realizedPnL )
        {
            this.reqId = reqId;
            this.dailyPnL = dailyPnL;
            this.unrealizedPnL = unrealizedPnL;
            this.realizedPnL = realizedPnL;
        }
    }
}
