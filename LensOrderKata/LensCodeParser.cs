using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    public class LensCodeParser
    {
        private readonly ILensRepository lensRepository;

        private readonly IInputParser inputParser;

        public LensCodeParser(ILensRepository lensRepository, IInputParser inputParser)
        {
            this.lensRepository = lensRepository;
            this.inputParser = inputParser;
        }

        /// <summary>
        /// Generates a formatted summary of the order, including itemized lens codes, quantities, individual totals,
        /// and the overall total price.
        /// </summary>
        public string CalculateOrderSummaryAsString(string InputString)
        {
            if (string.IsNullOrWhiteSpace(InputString))
            {
                throw new ArgumentException("Input cannot be empty.");
            }

            var codes = inputParser.Parse(InputString);
            var order = new LensOrder();
            var invalidCodes = new List<string>();

            foreach (var code in codes)
            {
                try
                {
                    var lens = lensRepository.GetByCode(code);
                    order.AddLens(lens);
                }
                catch (ArgumentException)
                {
                    invalidCodes.Add(code);
                }
            }

            if (invalidCodes.Count > 0)
            {
                throw new ArgumentException($"Lens with code {string.Join(", ", invalidCodes)} not found.");
            }

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
