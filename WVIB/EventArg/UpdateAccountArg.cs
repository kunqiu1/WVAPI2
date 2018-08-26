/* Copyright (C) 2018 Interactive Brokers LLC. All rights reserved. This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */
using System;

namespace WVIB
{
    public class UpdateAccountArg : EventArgs
    {
        public string key { get; set; }
        public string value { get; set; }
        public string currency { get; set; }
        public string accountName{ get; set; }


        public UpdateAccountArg(string key, string value, string currency, string accountName)
        {
            this.key = key;
            this.value = value;
            this.currency = currency;
            this.accountName = accountName;
        }
    }
}