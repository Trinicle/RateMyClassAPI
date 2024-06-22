using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RateMyClass.API.Entities;

namespace RateMyClass.API.DbContexts
{
    public class UniversityInfoContext : DbContext
    {
        public DbSet<University> Universities { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public UniversityInfoContext(DbContextOptions<UniversityInfoContext> options) : base(options) { }
    }
}
