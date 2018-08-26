using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVAPIDataModels
{
    public class IBAccountModel
    {
        public string key { get; set; }
        public string value { get; set; }
        public string currency { get; set; }
        public string accountName { get; set; }
        public DateTime LastUpdate { get; set; }

        public IBAccountModel(string key, string value, string currency, string accountName)
        {
            this.key = key;
            this.value = value;
            this.currency = currency;
            this.accountName = accountName;
        }
    }
}
