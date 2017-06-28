namespace XCentium.Plugins.Authorize.Net
{
    using Microsoft.Extensions.Configuration;
    using Sitecore.Commerce.Core;
    using System.Configuration;

    public class AuthorizeNetClientPolicy : Policy
    {
        public AuthorizeNetClientPolicy()
        {
            this.ApiLoginId = ConfigureSitecore.Configuration.GetSection("AppSettings:ApiLoginId").Value;
            this.TransactionKey = ConfigureSitecore.Configuration.GetSection("AppSettings:TransactionKey").Value;
        }

        public string ApiLoginId { get; set; }

        public string TransactionKey { get; set; }
    }
}
