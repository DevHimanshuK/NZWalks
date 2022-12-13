using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        //DbContext class Constructor
        //ctor 2*tab shortcut
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options) //passing options to the base class
        {

        }

        //3 DbSet type properties

        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; } //This is a lookup table so singular
    }
}
