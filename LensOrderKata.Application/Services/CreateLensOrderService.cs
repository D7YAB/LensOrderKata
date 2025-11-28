using LensOrderKata.Application.Interfaces;
using LensOrderKata.Core.Models;

namespace LensOrderKata.Application.Services
{
    /// <summary>
    /// Provides functionality to parse lens codes from input and construct a corresponding lens order.
    /// </summary>
    public class CreateLensOrderService
    {
        private readonly ILensRepository lensRepository;

        private readonly IInputParser inputParser;

        public CreateLensOrderService(ILensRepository lensRepository, IInputParser inputParser)
        {
            this.lensRepository = lensRepository;
            this.inputParser = inputParser;
        }

        /// <summary>
        /// Parses the specified input string and constructs a lens order containing all valid lenses referenced by
        /// code.
        /// </summary>
        public LensOrder ParseOrder(string input)
        {
            // Parse the input to extract lens codes
            var codes = inputParser.Parse(input);
            var order = new LensOrder();
            var invalidCodes = new List<string>();

            // Retrieve lenses by code and add them to the order
            foreach (var code in codes)
            {
                // Attempt to get the lens by code
                try
                {
                    var lens = lensRepository.GetByCode(code);
                    order.AddLens(lens);
                }
                // If the lens is not found, record the invalid code
                catch (ArgumentException)
                {
                    invalidCodes.Add(code);
                }
            }

            // If there are any invalid codes, throw an exception listing them
            if (invalidCodes.Count > 0)
                throw new ArgumentException($"Lens with code {string.Join(", ", invalidCodes)} not found.");

            return order;
        }
    }
}
