using LensOrderKata;
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

        /// <summary>
        /// Take a csv input of lens codes and ensure string can be parsed into individual codes.
        /// </summary>
        [TestCase("SV01, VF03, SV01, BF02", new[] { "SV01", "VF03", "SV01", "BF02" })]
        public void ParseInput_ShouldReturnIndividualCodes(string lensCodes, string[] expectedOutput)
        {
            // Arrange
            var parser = new LensCodeParser();

            // Act
            var result = parser.ParseCsvToCodes(lensCodes);
            // Assert
            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        /// <summary>
        /// Take a lens code and ensure the correct price is returned.
        /// </summary>
        [TestCase("SV01", 50)]
        [TestCase("BF02", 75)]
        [TestCase("VF03", 100)]
        public void LensCode_ShouldReturnPrice(string lensCode, double price)
        {
            // Arrange
            var parser = new LensCodeParser();
            // Act
            var result = parser.GetPriceForCode(lensCode);
            // Assert
            Assert.That(result, Is.EqualTo(price));
        }

        /// <summary>
        /// Ensure total cost is calculated correctly from a list of lens codes as string.
        /// </summary>
        [TestCase("SV01, VF03, SV01, BF02", 275)]
        public void CalculateTotalCost_ShouldReturnCorrectTotal(string input, double expectedTotal)
        {
            // Arrange
            var parser = new LensCodeParser();
            // Act
            var totalCost = parser.GetTotalPrice(input);
            // Assert
            Assert.That(totalCost, Is.EqualTo(expectedTotal));
        }

        /// <summary>
        /// Tests that the parser throws an exception when invalid lens codes are present in the input string.
        /// </summary>
        [TestCase("SV05")]
        public void SingleInvalidCode_ShouldThrowException(string code)
        {
            // Arrange
            var parser = new LensCodeParser();
            // Act and Assert
            Assert.Throws<ArgumentException>(() => parser.GetPriceForCode(code));
        }

        /// <summary>
        /// Ensure invalid lens code in input throws exception
        /// </summary>
        [TestCase("SV01, VF03, SV05, BF02")]
        public void InvalidCodeInList_ShouldThrowException(string input)
        {
            // Arrange
            var parser = new LensCodeParser();
            // Act and Assert
            Assert.Throws<ArgumentException>(() => parser.GetTotalPrice(input));
        }
    }
}
