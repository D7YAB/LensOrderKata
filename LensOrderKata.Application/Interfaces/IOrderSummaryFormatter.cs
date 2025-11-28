using LensOrderKata.Core.Models;

namespace LensOrderKata.Application.Interfaces
{
    /// <summary>
    /// Defines a method for formatting a summary of a lens order into a string representation.
    /// </summary>
    public interface IOrderSummaryFormatter
    {
        string Format(LensOrder order);
    }
}
