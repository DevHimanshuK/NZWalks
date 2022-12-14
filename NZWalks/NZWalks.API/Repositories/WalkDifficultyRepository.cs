using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            //Generate new GUID
            walkDifficulty.Id = Guid.NewGuid();
            //Add to DB
            await nZWalksDbContext.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            //Response
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            //Check if ID Exist
            var walkDiffToDelete = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(wd => wd.Id == id);
            if(walkDiffToDelete == null)
            {
                return null;
            }

            nZWalksDbContext.Remove(walkDiffToDelete);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDiffToDelete;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(wd => wd.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var WalkDiffToUpdate = await nZWalksDbContext.WalkDifficulty.FindAsync(id);

            //if null
            if(WalkDiffToUpdate == null)
            {
                return null;
            }

            WalkDiffToUpdate.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return WalkDiffToUpdate;
        }
    }
}
