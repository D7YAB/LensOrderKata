using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    public class InMemoryLensRepository : ILensRepository
    {
        private readonly List<Lens> lenses = new()
        {
            new Lens { Code = "SV01", Description = "Single Vision", Price = 50.0 },
            new Lens { Code = "BF02", Description = "Bifocal", Price = 75.0 },
            new Lens { Code = "VF03", Description = "Varifocal", Price = 100.0 }
        };

        public Lens? GetByCode(string code)
        {
            if (code == null)
            {
                return null;
            }

            var lens = lenses
                .FirstOrDefault(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

            return lens;
        }

        public IEnumerable<Lens> GetAll()
        {
            return lenses;
        }
    }

}
