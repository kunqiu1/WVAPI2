using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVAPIDataModels
{
    public class MktData
    {
        public int ReqId;
        public Dictionary<string, double> mktdata;

        public MktData(int id, string field, double value)
        {
            ReqId = id;
            mktdata = new Dictionary<string, double>();
            mktdata.Add(field, value);
        }
        public void AddUpdate(string field, double value)
        {
            if (mktdata.Keys.Contains(field))
                mktdata[field] = value;
            else
                mktdata.Add(field, value);
        }

    }
}
