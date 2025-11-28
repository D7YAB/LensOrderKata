using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    public interface IInputParser
    {
        List<string> Parse(string input);
    }
}
