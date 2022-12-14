using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //Create new GUID overwriting default coming from parameter.
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            //Check Whether walk with ID exist
            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);
            //If does now exist return NotFound
            if(existingWalk == null)
            {
                return null;
            }
            //If exist then delete and return success
            nZWalksDbContext.Walks.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            //Here currently we are only fetching walks,
            //return await nZWalksDbContext.Walks.ToListAsync();

            //but if we want to fetch the navigation properties as well then
            return await nZWalksDbContext.Walks
                .Include(w => w.Region)
                .Include(w => w.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Walks
                .Include(w => w.Region)
                .Include(w => w.WalkDifficulty)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            //find walk in DB
            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);

            //if not  null
            if (existingWalk != null)
            {
                existingWalk.Name= walk.Name;
                existingWalk.Length= walk.Length;
                existingWalk.Region= walk.Region;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                await nZWalksDbContext.SaveChangesAsync();
                return existingWalk;
            }
            //if null
            return null;
        }
    }
}
