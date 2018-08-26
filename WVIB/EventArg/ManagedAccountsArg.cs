/* Copyright (C) 2018 Interactive Brokers LLC. All rights reserved. This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */
using System;

namespace WVIB
{
    public class ManagedAccountsArg: EventArgs
    {
        public ManagedAccountsArg(string name)
        {
            AccountName = name;
        }

        public string AccountName { get; set; }
    }
}