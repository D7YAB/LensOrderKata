using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    public class LensOrder
    {
        private List<OrderedLens> orderedLenses = new List<OrderedLens>();
        public IReadOnlyList<OrderedLens> GetOrderedLenses()
        {
            // Return a read-only wrapper around the internal list
            return orderedLenses.AsReadOnly();
        }
        public double GetTotalPrice()
        {
            double total = 0.0;

            // Sum the total price of each ordered lens
            foreach (var orderedLens in orderedLenses)
            { 
                total += orderedLens.TotalPrice;
            }

            return total;
        }

        public void AddLens(Lens lens)
        {
            if (lens == null) throw new ArgumentNullException(nameof(lens));

            // Check if lens already exists in order
            var existing = orderedLenses.FirstOrDefault(l => l.Lens.Code.Equals(lens.Code, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.Quantity++; // Just increment the quantity
            }
            else
            {
                orderedLenses.Add(new OrderedLens(lens, 1));
            }
        }
    }
}
