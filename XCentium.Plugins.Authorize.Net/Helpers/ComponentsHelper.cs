namespace XCentium.Plugins.Authorize.Net
{
    using AuthorizeNet.Api.Contracts.V1;
    using Sitecore.Commerce.Core;

    /// <summary>
    ///  A Components Helper to translate party for address request
    /// </summary>
    public class ComponentsHelper
    {
        /// <summary>
        /// Translates the party to customerAddressType.
        /// </summary>
        /// <param name="party">The party.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal static protected customerAddressType TranslatePartyToAddressRequest(Party party, CommercePipelineExecutionContext context)
        {
            var address = new customerAddressType
            {
                firstName = party.FirstName,
                lastName = party.LastName,
                address = string.Concat(party.Address1, ",", party.Address2),
                city = party.City,
                state = party.State,
                country = party.Country,
                zip = party.ZipPostalCode
            };

            return address;
        }
    }
}