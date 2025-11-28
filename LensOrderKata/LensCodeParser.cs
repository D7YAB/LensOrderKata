using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    public class LensCodeParser
    {
        Lens[] lenses = new Lens[]
        {
            new Lens { Code = "SV01", Description = "Single Vision", Price = 50.0 },
            new Lens { Code = "BF02", Description = "Bifocal", Price = 75.0 },
            new Lens { Code = "VF03", Description = "Varifocal", Price = 100.0 }
        };

        /// <summary>
        /// Calculates the total price for a collection of lenses specified by their codes.
        /// </summary>
        public double GetTotalPrice(string lensCodes)
        {
            var lensCodesList = ParseCsvToCodes(lensCodes);
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
        /// Parses a comma-separated string and returns a list of individual code values.
        /// </summary>
        /// <param name="csv">A string containing code values separated by commas.</param>
        /// <returns>A list of strings representing the individual codes parsed from the input.</returns>
        public List<string> ParseCsvToCodes(string csv)
        {
            if(string.IsNullOrWhiteSpace(csv))
            {
                throw new ArgumentException($"Input cannot be empty.");
            }

            if (csv.Any(c => !char.IsLetterOrDigit(c) && c != ',' && !char.IsWhiteSpace(c)))
            {
                throw new ArgumentException("Invalid input detected. Please use commas to separate lens codes.");
            }
            var parsedCodes = new List<string>();

            if (!csv.Contains(","))
            {
                parsedCodes = csv.Split(' ').ToList();
                if (parsedCodes.Count>0 && GetLensByCode(csv.Trim()) is null)
                {
                    throw new ArgumentException("Invalid input detected. Please use commas to separate lens codes.");
                }
            }

            // Split the input string by commas and trim any whitespace from each code
            parsedCodes = csv.Split(',').Select(code => code.Trim()).ToList();

            return parsedCodes;
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

            var codes = ParseCsvToCodes(InputString);
            var invalidCodes = new List<string>();
            var lensCounts = new Dictionary<string, int>();

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
