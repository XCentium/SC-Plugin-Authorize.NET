// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// <summary>
//   SampleBlock pipeline block.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace XCentium.Plugins.Authorize.Net
{
    using Sitecore.Framework.Pipelines;
    using System.Threading.Tasks;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;

    public class SampleBlock : PipelineBlock<SampleArgument, SampleEntity, CommercePipelineExecutionContext>
    {
        private readonly SamplePipeline _pipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleBlock"/> class.
        /// </summary>
        /// <param name="pipeline">
        /// The SamplePipeline pipeline.
        /// </param>
        public SampleBlock(SamplePipeline pipeline)
        {
            this._pipeline = pipeline;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">
        /// The SampleArgument argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="SampleEntity"/>.
        /// </returns>
        public override Task<SampleEntity> Run(SampleArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull("The argument can not be null");
            var result = this._pipeline.Run(arg, context).Result;
            return Task.FromResult(result);
        }
    }
}
