// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureSitecore.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// <summary>
//   The SamplePlugin startup class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace XCentium.Plugins.Authorize.Net
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;
    using Sitecore.Commerce.Core;
    using System.Reflection;
    using Sitecore.Commerce.Plugin.Orders;
    using Microsoft.Extensions.Configuration;
    using System.IO;

    /// <summary>
    /// The carts configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup configuration sources.
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("config.json");

            Configuration = builder.Build();

            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
             .ConfigurePipeline<ICreateOrderPipeline>(d =>
             {
                 d.Replace<Plugin.Sample.Payments.Braintree.CreateFederatedPaymentBlock, Plugins.Authorize.Net.CreateFederatedPaymentBlock>()
                    .Before<CreateOrderBlock>();
             })
            );
            
        }
    }
}