using LensOrderKata.Application.Interfaces;

namespace LensOrderKata.Application.Services
{
    /// <summary>
    /// Provides functionality to process lens orders by parsing input data and formatting order summaries.
    /// </summary>
    public class LensOrderService
    {
        private readonly CreateLensOrderService createLensOrderService;
        private readonly IOrderSummaryFormatter formatter;

        public LensOrderService(CreateLensOrderService createLensOrderService, IOrderSummaryFormatter formatter)
        {
            this.createLensOrderService = createLensOrderService;
            this.formatter = formatter;
        }

        /// <summary>
        /// Processes the specified order input and returns a formatted representation of the order.
        /// </summary>
        public string ProcessOrder(string input)
        {
            var lensOrder = createLensOrderService.ParseOrder(input);
            return formatter.Format(lensOrder);
        }
    }
}
