// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SamplePolicy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// <summary>
//   SamplePolicy policy.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace XCentium.Plugins.Authorize.Net
{
    using Sitecore.Commerce.Core;

    public class SamplePolicy : Policy
    {
        /// <summary>
        /// Display text for the SampleEntity Display
        /// </summary>
        public string SampleEntityDisplay { get; set; }

        //TODO additional qualifications, policies, etc
        // Example: 
        //public int MinDisplayNameLength { get; set; }
        //public int MaxDisplayNameLength { get; set; }
    }
}
