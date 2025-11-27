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
        [Test]
        public void ParseInput_ShouldReturnIndividualCodes()
        {
            // Arrange
            var parser = new LensCodeParser();
            var lensCodes = "SV01, VF03, SV01, BF02";
            var expectedOutput = new List<string> { "SV01", "VF03", "SV01", "BF02" };
            // Act
            var result = parser.ParseCsvToCodes(lensCodes);
            // Assert
            Assert.That(result, Is.EqualTo(expectedOutput));
        }
    }
}
