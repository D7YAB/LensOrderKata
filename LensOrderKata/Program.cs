// See https://aka.ms/new-console-template for more information

using LensOrderKata.Application.Services;
using LensOrderKata.Infrastructure.Formatters;
using LensOrderKata.Infrastructure.Parsers;
using LensOrderKata.Infrastructure.Repositories;

var repository = new InMemoryLensRepository();
var inputParser = new StringCsvInputParser();
var formatter = new TextOrderSummaryFormatter();

var lensCodeParser = new CreateLensOrderService(repository, inputParser);
var orderService = new LensOrderService(lensCodeParser, formatter);

while (true)
{
    Console.Write("\nEnter lens order in CSV format (or 'exit' to quit): ");
    string input = Console.ReadLine();

    if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
        break;

    try
    {
        var result = orderService.ProcessOrder(input);
        Console.WriteLine("\nOrder Summary:");
        Console.WriteLine(result);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"\nError: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}