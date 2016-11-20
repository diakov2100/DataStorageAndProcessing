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
        // В этом методе можно заполнить таблицу по умолчанию
        protected override void Seed(Context context)
        {
            string[] SiteList = { "http://cwur.org/2012.php",
              "http://cwur.org/2013.php",
            "http://cwur.org/2014.php",
            "http://cwur.org/2015.php",
            "http://cwur.org/2016.php"};

            int RaitId = 1;
            foreach (string link in SiteList)
            {
                Raiting TecRaiting = new Raiting() { Id = RaitId, Year = int.Parse(link.Substring(16, 4)) };
                context.Raitings.Add(TecRaiting);
                context.SaveChanges();

                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string page = webClient.DownloadString(link);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(page);


                List<List<string>> table = doc.DocumentNode.SelectSingleNode("//table[@class='table table-bordered table-hover']")
                            .Descendants("tr")
                            .Skip(1)
                            .Where(tr => tr.Elements("td").Count() > 1)
                            .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                            .ToList();
                foreach (List<string> list in table)
                {
                    var tec = list[2];
                    Location TecLoc = context.Locations.FirstOrDefault(x => x.CountryName == tec);
                    if (TecLoc == null)
                    {
                        TecLoc = new Location()
                        {
                            Id = context.Locations.Count(),
                            CountryName = list[2]
                        };
                        context.Locations.AddOrUpdate(x => x.CountryName, TecLoc);
                        context.SaveChanges();
                    }

                    tec = list[1];
                    Institution TecInst = context.Institutions.FirstOrDefault(x => x.Name == tec);
                    if (TecInst == null)
                    {
                        TecInst = new Institution()
                        {
                            Id = context.Institutions.Count(),
                            Name = list[1],
                            Location = TecLoc,
                            LocationID = TecLoc.Id
                        };
                        context.Institutions.Add(TecInst);
                        context.SaveChanges();
                    }
                    if (TecRaiting.Year < 2014)
                    {
                        InstitutionRaiting TecInstRait = new InstitutionRaiting()
                        {
                            AlumniEmployment = STRparse(list[5]),
                            Citations = STRparse(list[9]),
                            Id = context.InstiutionRaitings.Count(),
                            Influence = STRparse(list[8]),
                            Institution = TecInst,
                            InstitutionID = TecInst.Id,
                            NationalRank = int.Parse(list[3]),
                            Patents = STRparse(list[10]),
                            Publications = STRparse(list[7]),
                            QualityofEducation = STRparse(list[4]),
                            QualityofFaculty = STRparse(list[6]),
                            Raiting = context.Raitings.First(x => x.Id == RaitId),
                            RaitingID = TecRaiting.Id,
                            Score = double.Parse(list[11].Replace('.', ',')),
                            WorldRank = int.Parse(list[0])
                        };
                        context.InstiutionRaitings.Add(TecInstRait);
                        context.SaveChanges();
                    }
                    else
                    {
                        NewInstitutionRaiting TecInstRait = new NewInstitutionRaiting()
                        {
                            AlumniEmployment = STRparse(list[5]),
                            Citations = STRparse(list[9]),
                            Id = context.InstiutionRaitings.Count(),
                            Influence = STRparse(list[8]),
                            Institution = TecInst,
                            InstitutionID = TecInst.Id,
                            NationalRank = int.Parse(list[3]),
                            Patents = STRparse(list[11]),
                            Publications = STRparse(list[7]),
                            QualityofEducation = STRparse(list[4]),
                            QualityofFaculty = STRparse(list[6]),
                            Raiting = context.Raitings.First(x => x.Id == RaitId),
                            RaitingID = TecRaiting.Id,
                            Score = double.Parse(list[12].Replace('.', ',')),
                            WorldRank = int.Parse(list[0]),
                            BroadImpact = int.Parse(list[10])
                        };
                        context.NewInstitutionsRaitings.Add(TecInstRait);
                        context.SaveChanges();
                    }
                }
                RaitId++;
            }
        }
        int STRparse(string str)
        {
            if (str.EndsWith("+"))
            {
                return int.Parse(str.Remove(str.Length - 1, 1));
            }
            return int.Parse(str);
        }
    }
}