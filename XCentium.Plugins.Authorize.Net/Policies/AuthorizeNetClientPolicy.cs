﻿namespace XCentium.Plugins.Authorize.Net
{
    using Sitecore.Commerce.Core;

    public class AuthorizeNetClientPolicy : Policy
    {
        public AuthorizeNetClientPolicy()
        {
            this.ApiLoginId = "87mzarLWY9T";
            this.TransactionKey = "352VhB2g322rK7aY";
        }

        public string ApiLoginId { get; set; }

        public string TransactionKey { get; set; }
    }
}