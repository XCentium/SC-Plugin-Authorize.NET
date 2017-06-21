namespace XCentium.Plugins.Authorize.Net
{
    using Sitecore.Framework.Pipelines;
    using System.Threading.Tasks;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;
    using System.Diagnostics;
    using AuthorizeNet.Api.Controllers;
    using System;
    using AuthorizeNet.Api.Contracts.V1;
    using AuthorizeNet.Api.Controllers.Bases;
    using Sitecore.Commerce.Plugin.Orders;
    using Sitecore.Commerce.Plugin.Payments;
    using System.Collections.Generic;
    using System.Linq;

    [PipelineDisplayName(PaymentsAuthorizeNetConstants.Pipelines.Blocks.CreateFederatedPaymentBlock)]
    public class CreateFederatedPaymentBlock : PipelineBlock<CartEmailArgument, CartEmailArgument, CommercePipelineExecutionContext>
    {

        public override Task<CartEmailArgument> Run(CartEmailArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull("The argument can not be null");

            var cart = arg.Cart;

            if (!cart.HasComponent<FederatedPaymentComponent>())
            {
                return Task.FromResult(arg);
            }

            var payment = cart.GetComponent<FederatedPaymentComponent>();

            if (string.IsNullOrEmpty(payment.PaymentMethodNonce))
            {
                context.Abort(context.CommerceContext.AddMessage(
                    context.GetPolicy<KnownResultCodes>().Error,
                    "InvalidOrMissingPropertyValue",
                    new object[] { "PaymentMethodNonce" },
                    $"Invalid or missing value for property 'PaymentMethodNonce'."), context);

                return Task.FromResult(arg);
            }       
            
            // TODO:  Move this to configuration?
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            
            // TODO:  Move to GetMerchantAuthenticationBlock?
            // define the merchant information (authentication / transaction id)
            var authorizeNetClientPolicy = new AuthorizeNetClientPolicy();
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = authorizeNetClientPolicy.ApiLoginId,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = authorizeNetClientPolicy.TransactionKey,
            };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // authorize and charge the card
                amount = payment.Amount.Amount,
                payment = new paymentType {
                    Item = new opaqueDataType
                    {
                        dataDescriptor = "COMMON.ACCEPT.INAPP.PAYMENT",
                        dataValue = payment.PaymentMethodNonce
                    }
                },
                billTo = ComponentsHelper.TranslatePartyToAddressRequest(payment.BillingParty, context),
                lineItems = (from cartLine in cart.Lines
                             select new lineItemType {
                                itemId = cartLine.ItemId.Split('|').ElementAt(1),
                                name = cartLine.Name == null ? cartLine.ItemId.Split('|').ElementAt(1) : cartLine.Name,
                                description = cartLine.ItemId,  
                                quantity = cartLine.Quantity,
                                unitPrice = cartLine.Totals.GrandTotal.Amount })
                                .ToArray()
            };
            
            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            //validate
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if (response.transactionResponse.messages != null)
                    {
                        payment.TransactionId = response.transactionResponse.transId;
                        payment.TransactionStatus = response.transactionResponse.responseCode;

                        Debug.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                        Debug.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                        Debug.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                        Debug.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                        Debug.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);

                        context.Logger.Log<string>(Microsoft.Extensions.Logging.LogLevel.Information, new Microsoft.Extensions.Logging.EventId(8788, "CreatePaymentSuccess"), $"{this.Name}  Create federated payment succeeded with Transaction Id: {response.transactionResponse.transId}", null, null);
                    }
                    else
                    {
                        Debug.WriteLine("Failed Transaction.");
                        if (response.transactionResponse.errors != null)
                        {
                            Debug.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            Debug.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                            context.Abort(context.CommerceContext.AddMessage(
                               context.GetPolicy<KnownResultCodes>().Error,
                               "CreatePaymentFailed", null,
                               $"{this.Name}. Create federated payment failed.  Error Code: {response.transactionResponse.errors[0].errorCode}.  Error Message: {response.transactionResponse.errors[0].errorText}."), context);

                        }
                    }
                }
                else
                {
                    Debug.WriteLine("Failed Transaction.");
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        Debug.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        Debug.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        context.Abort(context.CommerceContext.AddMessage(
                           context.GetPolicy<KnownResultCodes>().Error,
                           "CreatePaymentFailed", null,
                           $"{this.Name}. Create federated payment failed.  Error Code: {response.transactionResponse.errors[0].errorCode}.  Error Message: {response.transactionResponse.errors[0].errorText}."), context);

                    }
                    else
                    {
                        Debug.WriteLine("Error Code: " + response.messages.message[0].code);
                        Debug.WriteLine("Error message: " + response.messages.message[0].text);
                        context.Abort(context.CommerceContext.AddMessage(
                           context.GetPolicy<KnownResultCodes>().Error,
                           "CreatePaymentFailed", null,
                           $"{this.Name}. Create federated payment failed.  Error Code: {response.messages.message[0].code}.  Error Message: {response.messages.message[0].text}."), context);
                    }
                }
            }
            else
            {
                foreach (var result in controller.GetResults())
                {
                    context.Abort(context.CommerceContext.AddMessage(
                           context.GetPolicy<KnownResultCodes>().Error,
                           "CreatePaymentFailed", null,
                           $"{this.Name}. Create federated payment failed.  Error: {result}"), context);
                }
            }

            return Task.FromResult(arg);
            
        }
    }
}
