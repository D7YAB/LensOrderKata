using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    public class TextOrderSummaryFormatter : IOrderSummaryFormatter
    {
        public string Format(LensOrder order)
        {
            var output = new StringBuilder();
            foreach (var orderedLens in order.GetOrderedLenses())
            {
                output.AppendLine($"{orderedLens.Lens.Code} x{orderedLens.Quantity} = £{orderedLens.TotalPrice}");
            }
            output.Append($"Total = £{order.GetTotalPrice()}");
            return output.ToString();
        }
    }
}
