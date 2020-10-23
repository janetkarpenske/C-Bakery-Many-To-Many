
using Microsoft.EntityFrameworkCore;

namespace Bakery.Models
{
  public class BakeryContext : DbContext
  {
    //public DbSet<Item> Items { get; set; }

    public BakeryContext(DbContextOptions options) : base(options) { }
  }
}