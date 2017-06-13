namespace XCentium.Plugins.Authorize.Net
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;

    public class ChargeCreditCardArgument : PipelineArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChargeCreditCardArgument"/> class.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public ChargeCreditCardArgument(decimal amount)
        {
            Condition.Requires(amount).IsNotNull("The parameter can not be null");

            this.Amount = amount;
        }

        /// <summary>
        /// Gets or sets the Amount.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
