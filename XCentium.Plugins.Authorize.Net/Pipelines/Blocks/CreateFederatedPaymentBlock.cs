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

    public class CreateFederatedPaymentBlock : PipelineBlock<ChargeCreditCardArgument, createTransactionResponse, CommercePipelineExecutionContext>
    {

        public override Task<createTransactionResponse> Run(ChargeCreditCardArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull("The argument can not be null");

            var authorizeNetPolicy = new AuthorizeNetPolicy();
            var apiLoginId = authorizeNetPolicy.ApiLoginId;
            var transactionKey = authorizeNetPolicy.TransactionKey;

            var amount = arg.Amount;


            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = apiLoginId,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = transactionKey,
            };

            var creditCard = new creditCardType
            {
                cardNumber = "4111111111111111",
                expirationDate = "0718",
                cardCode = "123"
            };

            var billingAddress = new customerAddressType
            {
                firstName = "John",
                lastName = "Doe",
                address = "123 My St",
                city = "OurTown",
                zip = "98004"
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            // Add line Items
            var lineItems = new lineItemType[2];
            lineItems[0] = new lineItemType { itemId = "1", name = "t-shirt", quantity = 2, unitPrice = new Decimal(15.00) };
            lineItems[1] = new lineItemType { itemId = "2", name = "snowboard", quantity = 1, unitPrice = new Decimal(450.00) };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card

                amount = amount,
                payment = paymentType,
                billTo = billingAddress,
                lineItems = lineItems
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
                        Debug.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                        Debug.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                        Debug.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                        Debug.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                        Debug.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                    }
                    else
                    {
                        Debug.WriteLine("Failed Transaction.");
                        if (response.transactionResponse.errors != null)
                        {
                            Debug.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            Debug.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
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
                    }
                    else
                    {
                        Debug.WriteLine("Error Code: " + response.messages.message[0].code);
                        Debug.WriteLine("Error message: " + response.messages.message[0].text);
                    }
                }
            }
            else
            {
                Debug.WriteLine("Null Response.");
            }

            return Task.FromResult(response);
            
        }
    }
}
