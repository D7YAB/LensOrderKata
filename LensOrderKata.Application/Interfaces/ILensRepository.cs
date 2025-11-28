using LensOrderKata.Core.Models;

namespace LensOrderKata.Application.Interfaces
{
    /// <summary>
    /// Defines a contract for retrieving lens entities by code or as a collection.
    /// </summary>
    public interface ILensRepository
    {
        Lens? GetByCode(string code);
        IEnumerable<Lens> GetAll();
    }
}
