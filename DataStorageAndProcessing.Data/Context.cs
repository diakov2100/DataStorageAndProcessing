using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataStorageAndProcessing.Data
{
    public class Context : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<InstitutionRaiting> InstiutionRaitings { get; set; }
        public DbSet<NewInstitutionRaiting> NewInstitutionsRaitings { get; set; }
        public DbSet<Raiting> Raitings { get; set; }
        public Context() : base("RaitingDB")
        {
            Database.SetInitializer<Context>(new Initializer());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InstitutionRaiting>()
             .HasRequired(a => a.Institution);
            modelBuilder.Entity<InstitutionRaiting>()
             .HasRequired(a => a.Raiting);
            modelBuilder.Entity<NewInstitutionRaiting>()
             .HasRequired(a => a.Institution);
            modelBuilder.Entity<NewInstitutionRaiting>()
             .HasRequired(a => a.Raiting);
            modelBuilder.Entity<Institution>()
              .HasRequired(a => a.Location);
            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }
    }
    public class Initializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            FillDatabase.Fill(context);
            base.Seed(context);
        }
    }
}