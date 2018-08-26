/* Copyright (C) 2018 Interactive Brokers LLC. All rights reserved. This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */
using IBApi;
using System;
namespace WVIB
{
    public class UpdatePortfolioArg : EventArgs
    {
        public Contract contract;
        public double position;
        public double marketPrice;
        public double marketValue;
        public double averageCost;
        public double unrealizedPNL;
        public double realizedPNL;
        public string accountName;

        public UpdatePortfolioArg(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealizedPNL, double realizedPNL, string accountName)
        {
            this.contract = contract;
            this.position = position;
            this.marketPrice = marketPrice;
            this.marketValue = marketValue;
            this.averageCost = averageCost;
            this.unrealizedPNL = unrealizedPNL;
            this.realizedPNL = realizedPNL;
            this.accountName = accountName;
        }
        // For Cash
        public UpdatePortfolioArg(double value, string accountName)
        {
            contract = new Contract();
            contract.ConId = -1;
            contract.LocalSymbol = "CASH";
            contract.PrimaryExch = "CASH";
            contract.SecType = "CASH";
            position = 1;
            marketPrice = value;
            marketValue = value;
            averageCost = 0;
            unrealizedPNL = 0;
            realizedPNL = 0;
            this.accountName = accountName;
        }
    }
}