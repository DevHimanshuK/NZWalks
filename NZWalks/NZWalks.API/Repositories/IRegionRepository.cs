using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        //Creating Contract for region
        //synchronous call
        //IEnumerable<Region> GetAll();
        
        //Async Call
        Task <IEnumerable<Region>> GetAllAsync();
    }
}
