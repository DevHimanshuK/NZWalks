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

        // override on model creating method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.Role)
                .WithMany(y => y.User_Roles)
                .HasForeignKey(x => x.RoleId);
            
            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.User)
                .WithMany(y => y.User_Roles)
                .HasForeignKey(x => x.UserId);
        }

        //3 DbSet type properties
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; } //This is a lookup table so singular

        //User Table DBSet Properties
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; } 
        public DbSet<User_Role> Users_Roles { get; set; } 

    }
}
