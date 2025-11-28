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

        public LensOrder ParseOrder(string input)
        {
            var codes = inputParser.Parse(input);
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
                throw new ArgumentException($"Lens with code {string.Join(", ", invalidCodes)} not found.");

            return order;
        }
    }
}
