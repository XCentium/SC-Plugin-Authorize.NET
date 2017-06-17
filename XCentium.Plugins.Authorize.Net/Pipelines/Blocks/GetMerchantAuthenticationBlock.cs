namespace XCentium.Plugins.Authorize.Net.Pipelines.Blocks
{
    using System.Threading.Tasks;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;
    using System;

    /// <summary>
    ///  Defines a block which gets a payment service client tokent.
    /// </summary>
    /// <seealso>
    /// <cref>
    ///  Sitecore.Framework.Pipelines.PipelineBlock{System.String, System.String, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///  </cref>
    ///  </seealso>
    [PipelineDisplayName(PaymentsAuthorizeNetConstants.Pipelines.Blocks.GetMerchantAuthenticationBlock)]
    public class GetMerchantAuthenticationBlock : PipelineBlock<string, string, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// Runs the specified argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <param name="context">The context.</param>
        /// <returns>A client token string</returns>
        public override Task<string> Run(string arg, CommercePipelineExecutionContext context)
        {
            var authorizeNetClientPolicy = context.GetPolicy<AuthorizeNetClientPolicy>();
            if (authorizeNetClientPolicy == null)
            {
                context.CommerceContext.AddMessage(
                     context.GetPolicy<KnownResultCodes>().Error,
                     "InvalidOrMissingPropertyValue",
                     new object[] { "AuthorizeNetClientPolicy" },
                      $"{this.Name}. Missing AuthorizeNetClientPolicy");
                return Task.FromResult(arg);
            }

            if (string.IsNullOrEmpty(authorizeNetClientPolicy?.ApiLoginId) || string.IsNullOrEmpty(authorizeNetClientPolicy?.TransactionKey))
            {
                return Task.FromResult(string.Empty);
            }

            try
            {
                //var gateway = new BraintreeGateway(braintreeClientPolicy?.Environment, braintreeClientPolicy?.MerchantId, braintreeClientPolicy?.PublicKey, braintreeClientPolicy?.PrivateKey);
                //var clientToken = gateway.ClientToken.generate();
                return Task.FromResult(arg/*clientToken*/);
            }
            catch (Exception ex)
            {
                context.CommerceContext.AddMessage(
                   context.GetPolicy<KnownResultCodes>().Error,
                   "InvalidClientPolicy",
                   new object[] { "ClientPolicy" },
                    $"{this.Name}. Invalid ClientPolicy { ex.Message }");
                return Task.FromResult(arg);
            }
        }
    }
}