using LensOrderKata.Infrastructure.Formatters;
using LensOrderKata.Infrastructure.Parsers;
using LensOrderKata.Infrastructure.Repositories;
using LensOrderKata.Application.Services;
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
            var inputParser = new StringCsvInputParser();
            // Act
            var result = inputParser.Parse(lensCodes);
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
            var lensRepository = new InMemoryLensRepository();
            // Act
            var result = lensRepository.GetByCode(lensCode);
            // Assert
            Assert.That(result.Price, Is.EqualTo(price));
        }

        /// <summary>
        /// Ensure total cost is calculated correctly from a list of lens codes as string.
        /// </summary>
        [TestCase("SV01, VF03, SV01, BF02", 275)]
        public void CalculateTotalCost_ShouldReturnCorrectTotal(string input, double expectedTotal)
        {
            // Arrange
            var lensRepository = new InMemoryLensRepository();
            var inputParser = new StringCsvInputParser();
            var orderParser = new CreateLensOrderService(lensRepository, inputParser);
            // Act
            var order = orderParser.ParseOrder(input);
            var totalCost = order.GetTotalPrice();
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
            var lensRepository = new InMemoryLensRepository();

            // Act and Assert
            Assert.Throws<ArgumentException>(() => lensRepository.GetByCode(code));
        }

        /// <summary>
        /// Ensure invalid lens code in input throws exception
        /// </summary>
        [TestCase("SV01, VF03, SV05, BF02")]
        public void InvalidCodeInList_ShouldThrowException(string input)
        {
            // Arrange
            var lensRepository = new InMemoryLensRepository();
            var inputParser = new StringCsvInputParser();
            var createLensOrderService = new CreateLensOrderService(lensRepository, inputParser);
            // Act and Assert
            Assert.Throws<ArgumentException>(() => createLensOrderService.ParseOrder(input));
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
            var lensRepository = new InMemoryLensRepository();
            var inputParser = new StringCsvInputParser();
            var createLensOrderService = new CreateLensOrderService(lensRepository, inputParser);
            var lensOrderService = new LensOrderService(createLensOrderService, new TextOrderSummaryFormatter());

            // Act
            var summary = lensOrderService.ProcessOrder(csvInput);

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
            var inputParser = new StringCsvInputParser();
            var emptyInput = "";
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => inputParser.Parse(emptyInput));
            Assert.That(ex.Message, Is.EqualTo("Input cannot be empty."));
        }

        /// <summary>
        /// Ensures that an invalid lens code in the input string throws an ArgumentException.
        /// </summary>
        [TestCase("SV01, VF03, SV05, BF02")]
        public void InvalidCodeInSummary_ShouldThrowArgumentException(string input)
        {
            // Arrange
            var createLensOrderService = new CreateLensOrderService(new InMemoryLensRepository(), new StringCsvInputParser());
            var lensOrderService = new LensOrderService(createLensOrderService, new TextOrderSummaryFormatter());
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => lensOrderService.ProcessOrder(input));
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
            var createLensOrderService = new CreateLensOrderService(new InMemoryLensRepository(), new StringCsvInputParser());
            var lensOrderService = new LensOrderService(createLensOrderService, new TextOrderSummaryFormatter());
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => lensOrderService.ProcessOrder(input));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        /// <summary>
        /// Ensures that the order summary calculation correctly handles input strings containing extra whitespace
        /// between lens codes.
        /// </summary>
        [TestCase(" SV01 , VF03 ,  SV01 , BF02 ", "SV01 x2 = £100\r\nVF03 x1 = £100\r\nBF02 x1 = £75\r\nTotal = £275")]
        public void InputWithExtraWhitespace_ShouldReturnCorrectSummary(string input, string expectedOutput)
        {
            // Arrange
            var createLensOrderService = new CreateLensOrderService(new InMemoryLensRepository(), new StringCsvInputParser());
            var lensOrderService = new LensOrderService(createLensOrderService, new TextOrderSummaryFormatter());
            // Act & Assert
            var summary = lensOrderService.ProcessOrder(input);
            Assert.That(summary, Is.EqualTo(expectedOutput));
        }

        /// <summary>
        /// Ensures that duplicate lens codes in the input string are counted correctly when generating the order
        /// summary.
        /// </summary>
        [TestCase("SV01, SV01, SV01", "SV01 x3 = £150\r\nTotal = £150")]
        public void DuplicateCodes_ShouldCountCorrectly(string input, string expectedOutput)
        {
            var createLensOrderService = new CreateLensOrderService(new InMemoryLensRepository(), new StringCsvInputParser());
            var lensOrderService = new LensOrderService(createLensOrderService, new TextOrderSummaryFormatter());
            var summary = lensOrderService.ProcessOrder(input);
            Assert.That(summary, Is.EqualTo(expectedOutput));
        }

        /// <summary>
        /// Ensures that providing only invalid lens codes results in an ArgumentException containing all invalid codes
        /// in the exception message.
        /// </summary>
        [TestCase("XX01, YY02, ZZ03", "Lens with code XX01, YY02, ZZ03 not found.")]
        public void AllInvalidCodes_ShouldThrowExceptionWithAllInvalidCodes(string input, string expectedMessage)
        {
            var createLensOrderService = new CreateLensOrderService(new InMemoryLensRepository(), new StringCsvInputParser());
            var lensOrderService = new LensOrderService(createLensOrderService, new TextOrderSummaryFormatter());
            var ex = Assert.Throws<ArgumentException>(() => lensOrderService.ProcessOrder(input));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        /// <summary>
        /// Verifies that providing a single lens code input produces the expected order summary string.
        /// </summary>
        [TestCase("VF03", "VF03 x1 = £100\r\nTotal = £100")]
        public void SingleCodeInput_ShouldReturnCorrectSummary(string input, string expectedOutput)
        {
            var createLensOrderService = new CreateLensOrderService(new InMemoryLensRepository(), new StringCsvInputParser());
            var lensOrderService = new LensOrderService(createLensOrderService, new TextOrderSummaryFormatter());
            var summary = lensOrderService.ProcessOrder(input);
            Assert.That(summary, Is.EqualTo(expectedOutput));
        }

        /// <summary>
        /// Verifies that the lens code parser correctly handles input codes regardless of letter casing.
        /// </summary>
        [TestCase("sv01, vf03, SV01, bf02", "SV01 x2 = £100\r\nVF03 x1 = £100\r\nBF02 x1 = £75\r\nTotal = £275")]
        public void LowercaseInputCodes_ShouldBeHandledCorrectly(string input, string expectedOutput)
        {
            var createLensOrderService = new CreateLensOrderService(new InMemoryLensRepository(), new StringCsvInputParser());
            var lensOrderService = new LensOrderService(createLensOrderService, new TextOrderSummaryFormatter());
            var summary = lensOrderService.ProcessOrder(input);
            Assert.That(summary, Is.EqualTo(expectedOutput));
        }

        /// <summary>
        /// Verifies that parsing a CSV string with incorrect separators throws an ArgumentException.
        /// </summary>
        [TestCase("SV01;VF03;BF02")]
        [TestCase("SV01|VF03|BF02")]
        [TestCase("SV01 VF03 BF02")]
        public void ParseCsvToCodes_WrongSeparators_ShouldThrowException(string input)
        {
            var inputParser = new StringCsvInputParser();

            var ex = Assert.Throws<ArgumentException>(() => inputParser.Parse(input));
            Assert.That(ex.Message, Is.EqualTo("Invalid input detected. Please use commas to separate lens codes."));
        }

        /// <summary>
        /// Verifies that the order summary calculation throws an exception when lens codes are separated by invalid
        /// characters.
        /// </summary>
        [TestCase("SV01;VF03;BF02")]
        [TestCase("SV01|VF03|BF02")]
        [TestCase("SV01 VF03 BF02")]
        public void CalculateOrderSummary_WrongSeparators_ShouldThrowException(string input)
        {
            var createLensOrderService = new CreateLensOrderService(new InMemoryLensRepository(), new StringCsvInputParser());
            var lensOrderService = new LensOrderService(createLensOrderService, new TextOrderSummaryFormatter());

            var ex = Assert.Throws<ArgumentException>(() => lensOrderService.ProcessOrder(input));
            Assert.That(ex.Message, Is.EqualTo("Invalid input detected. Please use commas to separate lens codes."));
        }

        /// <summary>
        /// Verifies that parsing a CSV input string with leading or trailing commas throws an ArgumentException with
        /// the expected error message.
        /// </summary>
        [TestCase(",SV01,BF02,", "Input cannot contain empty codes.")]
        public void LeadingTrailingCommas_ShouldThrowException(string input, string expectedMessage)
        {
            var inputParser = new StringCsvInputParser();
            var ex = Assert.Throws<ArgumentException>(() => inputParser.Parse(input));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        /// <summary>
        /// Verifies that parsing a CSV input string containing repeated commas throws an ArgumentException with the
        /// expected error message.
        /// </summary>
        [TestCase("SV01,,BF02", "Input cannot contain empty codes.")]
        public void RepeatedCommas_ShouldThrowException(string input, string expectedMessage)
        {
            var inputParser = new StringCsvInputParser(); 
            var ex = Assert.Throws<ArgumentException>(() => inputParser.Parse(input));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        /// <summary>
        /// Verifies that the Parse method of StringCsvInputParser throws an ArgumentException when provided with a null
        /// input string.
        /// </summary>
        [Test]
        public void NullInput_ShouldThrowArgumentException()
        {
            var inputParser = new StringCsvInputParser();
            string input = null;
            var ex = Assert.Throws<ArgumentException>(() => inputParser.Parse(input));
            Assert.That(ex.Message, Is.EqualTo("Input cannot be empty."));
        }

        /// <summary>
        /// Verifies that parsing a string containing only whitespace throws an ArgumentException.
        /// </summary>
        [TestCase("    ")]
        [TestCase("\t\n")]
        public void WhitespaceOnlyInput_ShouldThrowArgumentException(string input)
        {
            var inputParser = new StringCsvInputParser();
            var ex = Assert.Throws<ArgumentException>(() => inputParser.Parse(input));
            Assert.That(ex.Message, Is.EqualTo("Input cannot be empty."));
        }

        /// <summary>
        /// Verifies that processing an order string containing mixed casing, extra whitespace, and duplicate lens codes
        /// produces the correct formatted summary.
        /// </summary>
        [TestCase(" sv01 , SV01 , vf03 ", "SV01 x2 = £100\r\nVF03 x1 = £100\r\nTotal = £200")]
        public void MixedCaseWhitespaceAndDuplicates_ShouldReturnCorrectSummary(string input, string expectedOutput)
        {
            var createLensOrderService = new CreateLensOrderService(new InMemoryLensRepository(), new StringCsvInputParser());
            var lensOrderService = new LensOrderService(createLensOrderService, new TextOrderSummaryFormatter());
            var summary = lensOrderService.ProcessOrder(input);
            Assert.That(summary, Is.EqualTo(expectedOutput));
        }

        /// <summary>
        /// Verifies that parsing a CSV input string with trailing whitespace after the last lens code works correctly.
        /// </summary>
        [TestCase("SV01, VF03   ", new[] { "SV01", "VF03" })]
        public void TrailingWhitespaceAfterLastCode_ShouldParseCorrectly(string input, string[] expected)
        {
            var inputParser = new StringCsvInputParser();
            var result = inputParser.Parse(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Verifies that parsing a string with mixed separators throws an ArgumentException.
        /// </summary>
        [TestCase("SV01, VF03; BF02")]
        [TestCase("SV01 | VF03, BF02")]
        public void MixedSeparators_ShouldThrowException(string input)
        {
            var inputParser = new StringCsvInputParser();
            var ex = Assert.Throws<ArgumentException>(() => inputParser.Parse(input));
            Assert.That(ex.Message, Is.EqualTo("Invalid input detected. Please use commas to separate lens codes."));
        }

        /// <summary>
        /// Verifies that parsing a string containing only separators throws an ArgumentException with the expected
        /// </summary>
        [TestCase(",", "Input cannot contain empty codes.")]
        [TestCase(",,", "Input cannot contain empty codes.")]
        [TestCase(" , , ", "Input cannot contain empty codes.")]
        public void OnlySeparators_ShouldThrowException(string input, string expectedMessage)
        {
            var inputParser = new StringCsvInputParser();
            var ex = Assert.Throws<ArgumentException>(() => inputParser.Parse(input));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }
    }
}
