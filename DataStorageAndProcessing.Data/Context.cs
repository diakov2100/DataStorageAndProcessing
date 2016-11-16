using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStorageAndProcessing.Data
{
    public class Context : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<InstitutionRaiting> InstiutionRaitings { get; set; }
        public DbSet<Raiting> Raitings { get; set; }
        public Context():base("RaitingDB")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Context>(null);
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InstitutionRaiting>()
             .HasRequired(a => a.Institution);
            modelBuilder.Entity<InstitutionRaiting>()
             .HasRequired(a => a.Raiting);
            modelBuilder.Entity<Institution>()
              .HasRequired(a => a.Location);
            base.OnModelCreating(modelBuilder);
        }
    }
}
