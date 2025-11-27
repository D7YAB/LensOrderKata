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
            var lens = lenses.FirstOrDefault(x => x.Code == code); ;
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

            return lens.Price;
        }

        /// <summary>
        /// Parses a comma-separated string and returns a list of individual code values.
        /// </summary>
        /// <param name="csv">A string containing code values separated by commas.</param>
        /// <returns>A list of strings representing the individual codes parsed from the input.</returns>
        public List<string> ParseCsvToCodes(string csv)
        {
            // Split the input string by commas and trim any whitespace from each code
            var parsedCodes = csv.Split(',').Select(code => code.Trim()).ToList();

            return parsedCodes;
        }
    }
}
