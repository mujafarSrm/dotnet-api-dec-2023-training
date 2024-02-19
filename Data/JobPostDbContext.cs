using DotNetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPI.Data
{
    public class JobPostDbContext : DbContext
    {
        public JobPostDbContext(DbContextOptions<JobPostDbContext> options): base(options)
        {

        }

        public DbSet<JobPost> JobPosts { get; set; }
    }
}