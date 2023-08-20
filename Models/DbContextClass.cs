using Microsoft.EntityFrameworkCore;
using core7_redis.Entities;

namespace core7_redis.Models
{
public class DbContextClass: DbContext {
        public DbContextClass(DbContextOptions < DbContextClass > options): base(options) {}
        public DbSet < User > Users { get; set; }
    }
    
}