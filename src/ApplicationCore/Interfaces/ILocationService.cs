using LocationSearch.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationSearch.ApplicationCore.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetLocationsAsync(LocationRequest request);
    }
}
