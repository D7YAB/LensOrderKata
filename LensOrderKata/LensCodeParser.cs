using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    public class LensCodeParser
    {
        /// <summary>
        /// Parses a comma-separated string and returns a list of individual code values.
        /// </summary>
        /// <param name="csv">A string containing code values separated by commas.</param>
        /// <returns>A list of strings representing the individual codes parsed from the input.</returns>
        public List<string> ParseCsvToCodes(string csv)
        {
            var parsedCodes = csv.Split(',').ToList();

            return parsedCodes;
        }
    }
}
