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

        /// <summary>
        /// Tests that a list of lens codes produces the correct summary and total cost.
        /// </summary>
        [TestCase(
            "SV01, VF03, SV01, BF02",
            "SV01 x2 = £100\r\nVF03 x1 = £100\r\nBF02 x1 = £75\r\nTotal = £275")]
        public void CalculateOrderSummary_ShouldReturnCorrectSummary(string csvInput, string expectedOutput)
        {
            // Arrange
            var parser = new LensCodeParser();

            // Act
            var summary = parser.CalculateOrderSummaryAsString(csvInput);

            // Assert
            Assert.That(summary, Is.EqualTo(expectedOutput));
        }

        /// <summary>
        /// Ensures that an empty input string throws an ArgumentException.
        /// </summary>
        [Test]
        public void EmptyInput_ShouldThrowArgumentException()
        {
            // Arrange
            var parser = new LensCodeParser();
            var emptyInput = "";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => parser.CalculateOrderSummaryAsString(emptyInput));
            Assert.That(ex.Message, Is.EqualTo("Input cannot be empty."));
        }

        /// <summary>
        /// Ensures that an invalid lens code in the input string throws an ArgumentException.
        /// </summary>
        [TestCase("SV01, VF03, SV05, BF02")]
        public void InvalidCodeInSummary_ShouldThrowArgumentException(string input)
        {
            // Arrange
            var parser = new LensCodeParser();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => parser.CalculateOrderSummaryAsString(input));
            Assert.That(ex.Message, Is.EqualTo("Lens with code SV05 not found."));
        }

        /// <summary>
        /// Ensures that multiple invalid lens codes in the input string throw an ArgumentException
        /// listing all invalid codes.
        /// </summary>
        [TestCase("SV01, XX01, VF03, YY02, BF02", "Lens with code XX01, YY02 not found.")]
        public void MultipleInvalidCodes_ShouldThrowArgumentExceptionWithAllInvalidCodes(string input, string expectedMessage)
        {
            // Arrange
            var parser = new LensCodeParser();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => parser.CalculateOrderSummaryAsString(input));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        /// <summary>
        /// Ensures that the order summary calculation correctly handles input strings containing extra whitespace
        /// between lens codes.
        /// </summary>
        [TestCase(" SV01 , VF03 ,  SV01 , BF02 ", "SV01 x2 = £100\r\nVF03 x1 = £100\r\nBF02 x1 = £75\r\nTotal = £275")]
        public void InputWithExtraWhitespace_ShouldReturnCorrectSummary(string input, string expectedOutput)
        {
            var parser = new LensCodeParser();
            var summary = parser.CalculateOrderSummaryAsString(input);
            Assert.That(summary, Is.EqualTo(expectedOutput));
        }

        /// <summary>
        /// Ensures that duplicate lens codes in the input string are counted correctly when generating the order
        /// summary.
        /// </summary>
        [TestCase("SV01, SV01, SV01", "SV01 x3 = £150\r\nTotal = £150")]
        public void DuplicateCodes_ShouldCountCorrectly(string input, string expectedOutput)
        {
            var parser = new LensCodeParser();
            var summary = parser.CalculateOrderSummaryAsString(input);
            Assert.That(summary, Is.EqualTo(expectedOutput));
        }

        /// <summary>
        /// Ensures that providing only invalid lens codes results in an ArgumentException containing all invalid codes
        /// in the exception message.
        /// </summary>
        [TestCase("XX01, YY02, ZZ03", "Lens with code XX01, YY02, ZZ03 not found.")]
        public void AllInvalidCodes_ShouldThrowExceptionWithAllInvalidCodes(string input, string expectedMessage)
        {
            var parser = new LensCodeParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.CalculateOrderSummaryAsString(input));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        /// <summary>
        /// Verifies that providing a single lens code input produces the expected order summary string.
        /// </summary>
        [TestCase("VF03", "VF03 x1 = £100\r\nTotal = £100")]
        public void SingleCodeInput_ShouldReturnCorrectSummary(string input, string expectedOutput)
        {
            var parser = new LensCodeParser();
            var summary = parser.CalculateOrderSummaryAsString(input);
            Assert.That(summary, Is.EqualTo(expectedOutput));
        }

    }
}
