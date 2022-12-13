using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        //To connect to the DB first we need to create a constructor
        public RegionRepository(NZWalksDbContext nZWalksDbContext) //Injecting NZWDbContext service created
        {
            this.nZWalksDbContext = nZWalksDbContext; // created and assigned private-readonly field
        }

        //Synchronous Implementation
        //public IEnumerable<Region> GetAll()
        //{
        //    return nZWalksDbContext.Regions.ToList();
        //}

        //async implementation
        public async Task< IEnumerable<Region>> GetAllAsync() 
        {
            return await nZWalksDbContext.Regions.ToListAsync();
        }

    }
}
