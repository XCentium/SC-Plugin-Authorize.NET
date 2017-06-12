// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleCommand.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// <summary>
//   Defines the SampleCommand command.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace XCentium.Plugins.Authorize.Net
{
    using System.Threading.Tasks;
    using Sitecore.Commerce.Core;
    using Core.Commands;

    /// <summary>
    /// Defines the SampleCommand command.
    /// </summary>
    public class SampleCommand : CommerceCommand
    {
        private readonly SamplePipeline _pipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleCommand"/> class.
        /// </summary>
        /// <param name="pipeline">
        /// The pipeline.
        /// </param>
        public SampleCommand(SamplePipeline pipeline)
        {
            this._pipeline = pipeline;
        }

        /// <summary>
        /// The process of the command
        /// </summary>
        /// <param name="commerceContext">
        /// The commerce context
        /// </param>
        /// <param name="parameter">
        /// The parameter for the command
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<SampleEntity> Process(CommerceContext commerceContext, object parameter)
        {
            using (var activity = CommandActivity.Start(commerceContext, this))
            {
                var arg = new SampleArgument(parameter);
                var result = this._pipeline.Run(arg, new CommercePipelineExecutionContextOptions(commerceContext));

                return Task.FromResult(result.Result);
            }
        }
    }
}