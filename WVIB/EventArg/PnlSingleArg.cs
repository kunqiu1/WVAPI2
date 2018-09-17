using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVIB
{
    public class PnlSingleArg
    {
        public int reqId;
        public int pos;
        public double dailyPnL;
        public double unrealizedPnL;
        public double realizedPnL;
        public double value;

        public PnlSingleArg(int reqId, int pos, double dailyPnL, double unrealizedPnL, double realizedPnL, double value)
        {
            this.reqId = reqId;
            this.pos = pos;
            this.dailyPnL = dailyPnL;
            this.unrealizedPnL = unrealizedPnL;
            this.realizedPnL = realizedPnL;
            this.value = value;
        }
    }
}
