using System;
using System.Collections.Generic;
using System.Text;

namespace LensOrderKata
{
    public interface ILensRepository
    {
        Lens? GetByCode(string code);
        IEnumerable<Lens> GetAll();
    }
}
