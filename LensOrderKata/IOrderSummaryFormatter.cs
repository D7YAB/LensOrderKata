using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    public interface IOrderSummaryFormatter
    {
        string Format(LensOrder order);
    }
}
