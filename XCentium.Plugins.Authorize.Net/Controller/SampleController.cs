// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleController.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// <summary>
//   Defines the SampleController controller for the sample plugin.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace XCentium.Plugins.Authorize.Net
{
    using Microsoft.AspNetCore.Mvc;
    using Sitecore.Commerce.Core;
    using System;

    [Microsoft.AspNetCore.OData.EnableQuery]
    [Route("api/Sample")]
    public class SampleController : CommerceController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleController"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="globalEnvironment">The global environment.</param>
        public SampleController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment) : base(serviceProvider, globalEnvironment)
        {
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("(Id={id})")]
        [Microsoft.AspNetCore.OData.EnableQuery]
        public IActionResult Get(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var result = Command<SampleCommand>()?.Process(CurrentContext, id);
            if (result?.Result == null)
            {
                return NotFound();
            }

            return new ObjectResult(result.Result);
        }
    }
}
