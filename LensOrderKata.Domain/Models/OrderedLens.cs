namespace LensOrderKata.Core.Models
{
    /// <summary>
    /// Represents a lens item and the quantity ordered, including the total price for the order.
    /// </summary>
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
