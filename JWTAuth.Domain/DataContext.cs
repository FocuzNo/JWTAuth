using JWTAuth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.Domain
{
    public sealed class DataContext : DbContext
    {
        public DbSet<RegisterUser> RegisterUsers { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options)
            : base(options){
        }
    }
}
