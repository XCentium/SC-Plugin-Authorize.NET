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
                /// The get client token block name.
                /// </summary>
                public const string GetClientTokenBlock = "PaymentsAuthorizeNET.block.getclienttoken";

                /// <summary>
                /// The add federated payment block
                /// </summary>
                public const string UpdateFederatedPaymentBlock = "PaymentsAuthorizeNET.block.updatefederatedpayment";

                /// <summary>
                /// The update federated payment after settlement block
                /// </summary>
                public const string UpdateFederatedPaymentAfterSettlementBlock = "PaymentsAuthorizeNET.block.updatefederatedpaymentaftersettlement";

                /// <summary>
                /// The create federated payment block
                /// </summary>
                public const string CreateFederatedPaymentBlock = "PaymentsAuthorizeNET.block.createfederatedpayment";

                /// <summary>
                /// The ensure settle payment requested block
                /// </summary>
                public const string EnsureSettlePaymentRequestedBlock = "PaymentsAuthorizeNET.block.ensuresettlepaymentrequested";

                /// <summary>
                /// The void federated payment block
                /// </summary>
                public const string VoidFederatedPaymentBlock = "PaymentsAuthorizeNET.block.voidfederatedpayment";

                /// <summary>
                /// The void cancel order federated payment block
                /// </summary>
                public const string VoidCancelOrderFederatedPaymentBlock = "PaymentsAuthorizeNET.block.voidcancelorderfederatedpayment";

                /// <summary>
                /// The refund federated payment block
                /// </summary>
                public const string RefundFederatedPaymentBlock = "PaymentsAuthorizeNET.block.refundfederatedpayment";

                /// <summary>
                /// The void on hold order federated payment block
                /// </summary>
                public const string VoidOnHoldOrderFederatedPaymentBlock = "PaymentsAuthorizeNET.block.voidonholdorderfederatedpayment";

                /// <summary>
                /// The validate settlement block
                /// </summary>
                public const string ValidateSettlementBlock = "PaymentsAuthorizeNET.block.validatesettlement";
            }
        }
    }
}
