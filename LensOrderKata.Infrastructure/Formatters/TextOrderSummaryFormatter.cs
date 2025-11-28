using LensOrderKata.Application.Interfaces;
using LensOrderKata.Core.Models;
using System.Text;

namespace LensOrderKata.Infrastructure.Formatters
{
    /// <summary>
    /// Provides functionality to format an order summary as plain text for lens orders.
    /// </summary>
    public class TextOrderSummaryFormatter : IOrderSummaryFormatter
    {
        /// <summary>
        /// Generates a formatted string summarizing the lenses in the specified order, including individual lens
        /// details and the total price.
        /// </summary>
        public string Format(LensOrder order)
        {
            var output = new StringBuilder();

            // Append each ordered lens's details to the output
            foreach (var orderedLens in order.GetOrderedLenses())
            {
                output.AppendLine($"{orderedLens.Lens.Code} x{orderedLens.Quantity} = £{orderedLens.TotalPrice}");
            }

            // Append the total price of the order
            output.Append($"Total = £{order.GetTotalPrice()}");

            return output.ToString();
        }
    }
}
