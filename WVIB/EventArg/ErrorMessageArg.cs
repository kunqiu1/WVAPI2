/* Copyright (C) 2018 Interactive Brokers LLC. All rights reserved. This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */
using System;
namespace WVIB
{
    public class ErrorMessageArg : EventArgs
    {
        private int id;
        private int errorCode;
        private string errorMsg;

        public ErrorMessageArg(int id, int errorCode, string errorMsg)
        {
            this.id = id;
            this.errorCode = errorCode;
            this.errorMsg = errorMsg;
        }
    }
}