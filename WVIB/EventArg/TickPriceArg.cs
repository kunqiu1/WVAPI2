/* Copyright (C) 2018 Interactive Brokers LLC. All rights reserved. This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */
using IBApi;
using System;
namespace WVIB
{
    public class TickPriceArg : EventArgs
    {
        public int tickerId;
        public int field;
        public double price;
        public TickAttrib attribs;

        public TickPriceArg(int tickerId, int field, double price, TickAttrib attribs)
        {
            this.tickerId = tickerId;
            this.field = field;
            this.price = price;
            this.attribs = attribs;
        }
    }
}