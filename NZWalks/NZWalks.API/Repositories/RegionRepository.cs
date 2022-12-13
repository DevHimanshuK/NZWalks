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

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await nZWalksDbContext.Regions.FirstOrDefaultAsync(reg => reg.Id == id);
            if (region == null)
            {
                return null;
            }

            //Delete the region
            nZWalksDbContext.Remove(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
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

        public async Task<Region> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(reg => reg.Id  == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            //Check whether region exist
            var regionToUpdate = await nZWalksDbContext.Regions.FirstOrDefaultAsync(reg => reg.Id == id);
            //if not respond null
            if(regionToUpdate == null)
            {
                return null;
            }
            //if exist map and update region
            regionToUpdate.Code = region.Code;
            regionToUpdate.Name = region.Name;
            regionToUpdate.Area= region.Area;
            regionToUpdate.Lat= region.Lat;
            regionToUpdate.Long= region.Long;
            regionToUpdate.Population= region.Population;

            await nZWalksDbContext.SaveChangesAsync();
            //return updated region

            return regionToUpdate;
        }
    }
}
