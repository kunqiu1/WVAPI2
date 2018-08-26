/* Copyright (C) 2018 Interactive Brokers LLC. All rights reserved. This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */
using System;
 namespace WVIB
{
    public class TickOptionComputationArg :EventArgs
    {
        public int tickerId;
        public int field;
        public double impliedVolatility;
        public double delta;
        public double gamma;
        public double vega;
        public double theta;
        public double pvDividend;
        public double undPrice;
        public double optPrice;

        public TickOptionComputationArg(int tickerId, int field, double impliedVolatility, double delta, double gamma, double vega, double theta, double pvDividend, double undPrice, double optPrice)
        {
            this.tickerId = tickerId;
            this.field = field;
            this.impliedVolatility = impliedVolatility;
            this.delta = delta;
            this.gamma = gamma;
            this.vega = vega;
            this.theta = theta;
            this.pvDividend = pvDividend;
            this.undPrice = undPrice;
            this.optPrice = optPrice;
        }
    }
}