using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Collections.Generic;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        // You’re building a basic lens ordering system for an optometry practice. Each lens type has a code and a price.

        //Lens Codes
        //Code Description     Price
        //SV01 Single Vision   £50
        //BF02 Bifocal         £75
        //VF03 Varifocal       £100

        //Requirements
        //•Accept a list of lens codes.
        //•Calculate the total cost.
        //•Ignore invalid codes.
        //•Return a summary of lens types and total cost.

        //Example Input
        //SV01, VF03, SV01, BF02

        //Expected Output
        //SV01 x2 = £100
        //VF03 x1 = £100
        //BF02 x1 = £75
        //Total = £275
    }
}
