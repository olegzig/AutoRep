using Microsoft.EntityFrameworkCore;

namespace AutoRep.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AutoRep.Models.Work> Work { get; set; }
        public DbSet<AutoRep.Models.WorkType> WorkType { get; set; }
        public DbSet<AutoRep.Models.UserRequest> Request { get; set; }
        public DbSet<AutoRep.Models.MachineParts> MachineParts { get; set; }
    }
}