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

        public bool GetPriceForCode(string lensCode)
        {
            throw new NotImplementedException();
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
