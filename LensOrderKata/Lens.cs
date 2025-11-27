using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    /// <summary>
    /// Represents a lens with its code, description, and price.
    /// </summary>
    public class Lens
    {
        string Code { get; set; } = string.Empty;
        string Description { get; set; } = string.Empty;
        double Price { get; set; } = 0.0;
    }
}
