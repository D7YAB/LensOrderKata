namespace LensOrderKata.Application.Interfaces
{
    /// <summary>
    /// Defines a contract for parsing input strings into a collection of tokens or segments.
    /// </summary>
    public interface IInputParser
    {
        List<string> Parse(string input);
    }
}
