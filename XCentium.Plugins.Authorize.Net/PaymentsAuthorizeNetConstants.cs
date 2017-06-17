namespace XCentium.Plugins.Authorize.Net
{
    /// <summary>
    /// The payments constants.
    /// </summary>
    public static class PaymentsAuthorizeNetConstants
    {
        /// <summary>
        /// The name of the payments pipelines.
        /// </summary>
        public static class Pipelines
        {
            /// <summary>
            /// The name of the payment pipelines blocks.
            /// </summary>
            public static class Blocks
            {
                /// <summary>
                /// The get merchant authentication block name.
                /// </summary>
                public const string GetMerchantAuthenticationBlock = "PaymentsAuthorizeNET.block.getmerchantauthentication";

                /// <summary>
                /// The charge credit card block name.
                /// </summary>
                public const string CreateFederatedPaymentBlock = "PaymentsAuthorizeNET.block.createfederatedpaymentblock";

                public const string CreateCustomerProfileBlock = "PaymentsAuthorizeNET.block.createcustomerprofileblock";
                
            }
        }
    }
}
