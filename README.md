# SC-Plugin-Authorize.NET
## XCentium Sitecore Commerce Authorize.NET Payment Plugin

The Authorize.NET plugin is a Sitecore Commerce plugin which is used to itegrate the Authorize.NET AcceptJS (see http://developer.authorize.net/api/reference/features/acceptjs.html) method of payment.  See http://developer.authorize.net/api/reference/index.html#payment-transactions-create-an-accept-payment-transaction for more information about the Authorize.NET APIs used.

## Using the plugin

1. Create or use an existing solution containing the Sitecore.Commerce.Engine.xproj from the Sitecore Commerce SDK, and any other dependencies. 
2. Clone this repo.
3. Incude the XCentium.Plugins.Authorize.Net.xproj in your solution.
4. Add the XCentium.Plugins.Authorize.Net project to the Sitecore.Commerce.Engine references.
5. Edit the Startup constructor of the Sitecore.Commerce.Engine to set the current directory to the web root path (this is to allow the configuration APIs to work for plugins):

    public Startup(IServiceProvider serviceProvider, IHostingEnvironment hostEnv, ILoggerFactory loggerFactory)
    {
        this._hostEnv = hostEnv;
        this._hostServices = serviceProvider;

        Directory.SetCurrentDirectory(this._hostEnv.WebRootPath);
        
        ...
    }
 
