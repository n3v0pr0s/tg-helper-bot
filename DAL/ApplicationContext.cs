using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationContext : DbContext
    {       
        public DbSet<User> tg_users { get; set; }
        public DbSet<Note> tg_notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {           
            var host = "ec2-54-72-155-238.eu-west-1.compute.amazonaws.com";
            var port = "5432";          
            var db = "d27n3usq45obc6";           
            var pass = "d0890400403bcdfd77690802122dbe8959d14a8edf8c33a05e001144cc8ab7dd";
            var user = "gpbwkvkcvqkdua";

            optionsBuilder.UseNpgsql($"User ID={user};Host={host};Port={port};Database={db};Password={pass};sslmode=Require;Trust Server Certificate=true;");
        }
    }
}
