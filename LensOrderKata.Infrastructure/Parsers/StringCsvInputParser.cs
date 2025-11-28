using LensOrderKata.Application.Interfaces;

namespace LensOrderKata.Infrastructure.Parsers
{
    /// <summary>
    /// Provides functionality to parse a comma-separated string input into a list of codes, validating the format and
    /// allowed characters.
    /// </summary>
    public class StringCsvInputParser : IInputParser
    {
        public List<string> Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be empty.");
            }

            // Only allow letters, digits, commas, and whitespace
            if (input.Any(c => !char.IsLetterOrDigit(c) && c != ',' && !char.IsWhiteSpace(c)))
            {
                throw new ArgumentException("Invalid input detected. Please use commas to separate lens codes.");
            }

            // Trim the input first
            input = input.Trim();

            // Detect space-separated codes (no commas but multiple entries)
            if (!input.Contains(","))
            {
                var spaceSplit = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (spaceSplit.Length > 1)
                {
                    throw new ArgumentException("Invalid input detected. Please use commas to separate lens codes.");
                }
            }

            // Split by commas, trim whitespace, remove empty entries
            var parsedCodes = input
                .Split(',')
                .Select(code => code.Trim())
                .ToList();

            // Check for empty codes after trimming
            if (parsedCodes.Any(string.IsNullOrEmpty))
            {
                throw new ArgumentException("Input cannot contain empty codes.");
            }

            return parsedCodes;
        }
    }
}
