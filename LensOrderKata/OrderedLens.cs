using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    public class OrderedLens
    {
        public Lens Lens { get; }
        public int Quantity { get; set; }

        public double TotalPrice => Lens.Price * Quantity;

        public OrderedLens(Lens lens, int quantity)
        {
            Lens = lens;
            Quantity = quantity;
        }
    }
}
