using LensOrderKata.Application.Interfaces;
using LensOrderKata.Core.Models;

namespace LensOrderKata.Infrastructure.Repositories
{
    /// <summary>
    /// Provides an in-memory implementation of the <see cref="ILensRepository"/> interface for managing and retrieving
    /// lens data.
    /// </summary>
    public class InMemoryLensRepository : ILensRepository
    {
        private readonly List<Lens> lenses = new()
        {
            new Lens { Code = "SV01", Description = "Single Vision", Price = 50.0 },
            new Lens { Code = "BF02", Description = "Bifocal", Price = 75.0 },
            new Lens { Code = "VF03", Description = "Varifocal", Price = 100.0 }
        };

        /// <summary>
        /// Retrieves a lens from the collection that matches the specified code, using a case-insensitive comparison.
        /// </summary>
        public Lens? GetByCode(string code)
        {
            // Trim whitespace from the input code
            code = code.Trim();

            // Find the lens with a case-insensitive match
            var lens = lenses
                .FirstOrDefault(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

            // Throw an exception if the lens is not found
            if (lens == null)
            {
                throw new ArgumentException($"Lens with code {code} not found.");
            }

            return lens;
        }

        /// <summary>
        /// Retrieves all available lens objects in the collection.
        /// </summary>
        public IEnumerable<Lens> GetAll()
        {
            // Return the complete list of lenses
            return lenses;
        }
    }

}
