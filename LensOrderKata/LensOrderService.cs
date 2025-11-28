using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    public class LensOrderService
    {
        private readonly LensCodeParser parser;
        private readonly IOrderSummaryFormatter formatter;

        public LensOrderService(LensCodeParser parser, IOrderSummaryFormatter formatter)
        {
            this.parser = parser;
            this.formatter = formatter;
        }

        public string ProcessOrder(string input)
        {
            var order = parser.ParseOrder(input);
            return formatter.Format(order);
        }
    }
}
