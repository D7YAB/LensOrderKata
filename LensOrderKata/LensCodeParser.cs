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
        /// Calculates the total price for a collection of lenses specified by their codes.
        /// </summary>
        public double GetTotalPrice(string lensCodes)
        {
            var lensCodesList = inputParser.Parse(lensCodes);
            double total = 0.0;
            foreach (var code in lensCodesList)
            {
                var price = GetPriceForCode(code);
                total += price;
            }
            return total;
        }

        /// <summary>
        /// Retrieves the lens associated with the specified code.
        /// </summary>
        /// <param name="code">The unique code identifying the lens to retrieve. Cannot be null.</param>
        /// <returns>The lens that matches the specified code, or null if no matching lens is found.</returns>
        public Lens? GetLensByCode(string code)
        {
            var lenses = lensRepository.GetAll();
            var lens = lenses.FirstOrDefault(x => string.Equals(x.Code, code, StringComparison.OrdinalIgnoreCase));
            return lens;
        }

        /// <summary>
        /// Retrieves the price associated with the specified lens code.
        /// </summary>
        /// <param name="lensCode">code identifying the lens for which to obtain the price</param>
        /// <returns>The price of the lens corresponding to the provided code.</returns>
        public double GetPriceForCode(string lensCode)
        {
            var lens = GetLensByCode(lensCode);

            if(lens == null)
            {
                throw new ArgumentException($"Lens with code {lensCode} not found.");
            }

            return lens.Price;
        }

        /// <summary>
        /// Generates a formatted summary of the order, including itemized lens codes, quantities, individual totals,
        /// and the overall total price.
        /// </summary>
        public string CalculateOrderSummaryAsString(string InputString)
        {
            if (string.IsNullOrWhiteSpace(InputString))
            {
                throw new ArgumentException($"Input cannot be empty.");
            }

            var codes = inputParser.Parse(InputString);
            var invalidCodes = new List<string>();
            var lensCounts = new Dictionary<string, int>();

            if (codes.Any(string.IsNullOrEmpty))
            {
                throw new ArgumentException($"Input cannot contain empty codes.");
            }

            foreach (var code in codes)
            {
                try 
                {
                    GetPriceForCode(code);
                }
                catch (ArgumentException)
                {
                    invalidCodes.Add(code);
                    continue;
                }

                if (!lensCounts.ContainsKey(code))
                {
                    var lens = GetLensByCode(code);
                    lensCounts.Add(lens.Code, 1);
                }
                else
                {
                    lensCounts[code]++;
                }
            }

            // Throw if there are invalid codes
            if (invalidCodes.Any())
            {
                throw new ArgumentException($"Lens with code {string.Join(", ", invalidCodes)} not found.");
            }

            var total = GetTotalPrice(InputString);
            var output = string.Empty;

            foreach (var lens in lensCounts)
            {
                var lensDetails = GetLensByCode(lens.Key);
                if(lensDetails != null)
                {
                    output += $"{lensDetails.Code} x{lens.Value} = £{lensDetails.Price * lens.Value}\r\n";
                }
            }
            output += $"Total = £{total}";

            return output;
        }
    }
}
