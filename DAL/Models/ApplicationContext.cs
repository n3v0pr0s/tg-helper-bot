using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Case> cases { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Note> notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = "ec2-50-19-247-157.compute-1.amazonaws.com";
            var port = "5432";
            var db = "d9m29ifk15mc3q;Username=hulsprrrynffyb";
            var pass = "2cfcc8362797203bb6527543223e413ea423f4bf6c8306ce6fc7ebfd912988f4";

            optionsBuilder.UseNpgsql($"Host={host};Port={port};Database={db};Password={pass};sslmode=Require;Trust Server Certificate=true;");
        }
    }
}
