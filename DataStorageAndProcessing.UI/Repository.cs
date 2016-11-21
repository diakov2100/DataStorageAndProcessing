using DataStorageAndProcessing.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStorageAndProcessing.UI
{
    public class Repository
    {
        public class YearRait
        {
            public int WordRank { get; set; }
            public string Institution { get; set; }
            public string Location { get; set; }
            public int NationalRank { get; set; }
            public int QualityofEducation { get; set; }
            public int AlumniEmployment { get; set; }
            public int QualityofFaculty { get; set; }
            public int Publications { get; set; }
            public int Influence { get; set; }
            public int Citations { get; set; }
            public int Patents { get; set; }
            public double Score { get; set; }
        }
        public class NewYearRait : YearRait
        {
            public int BroadImpact { get; set; }
        }

        public class YearRaitGroup
        {
            public string Location { get; set; }
            public IEnumerable<YearRait> Institutions { get; set; }
        }
        public class NewYearRaitGroup
        {
            public string Location { get; set; }
            public IEnumerable<NewYearRait> Institutions { get; set; }
        }
        public class UniversityDynamic
        {
            public string Institution { get; set; }
            public int Year { get; set; }
            public int WorldRank { get; set; }
            public double Score { get; set; }
        }
        public class UniversityScore
        {
            public string Institution { get; set; }
            public double MinScore { get; set; }
            public double MaxScore { get; set; }
            public double AverScore { get; set; }
        }
        public static void InitilizeDB()
        {
            using (var contex = new Context())
            {
                contex.Database.Initialize(true);
                contex.SaveChanges();
            }
        }
        public static void DeleteDB()
        {
            using (var context = new Context())
            {
                context.Database.Delete();
            }
        }
        public static object GetFullRank(int year, bool fl)
        {
            using (var context = new Context())
            {

                if (year < 2014)
                {
                    IEnumerable<Repository.YearRait> Result = new List<YearRait>();
                    IEnumerable<Repository.YearRait> sequence = from t1 in context.Raitings
                                                                where t1.Year == year
                                                                join t2 in context.InstiutionRaitings on t1.Id equals t2.RaitingID
                                                                join t3 in context.Institutions on t2.InstitutionID equals t3.Id
                                                                join t4 in context.Locations on t3.LocationID equals t4.Id
                                                                orderby t2.WorldRank ascending
                                                                select new Repository.YearRait
                                                                {
                                                                    WordRank = t2.WorldRank,
                                                                    Institution = t3.Name,
                                                                    Location = t4.CountryName,
                                                                    NationalRank = t2.NationalRank,
                                                                    QualityofEducation = t2.QualityofEducation,
                                                                    AlumniEmployment = t2.AlumniEmployment,
                                                                    QualityofFaculty = t2.QualityofFaculty,
                                                                    Publications = t2.Publications,
                                                                    Influence = t2.Influence,
                                                                    Citations = t2.Citations,
                                                                    Patents = t2.Patents,
                                                                    Score = t2.Score
                                                                };
                    if (fl == true)
                    {
                        IEnumerable<Repository.YearRaitGroup> sequence2 = from t1 in sequence
                                                                          group t1 by t1.Location into g
                                                                          orderby g.Key descending
                                                                          select new Repository.YearRaitGroup
                                                                          {

                                                                              Location = g.Key,
                                                                              Institutions = g.OrderByDescending(x => x.Score)
                                                                          };

                        foreach (var tec in sequence2)
                        {
                            if (Result.Count() != 0)
                                Result = tec.Institutions.Union(Result);
                            else Result = tec.Institutions;
                        }
                    }
                    else
                    {
                        Result = sequence;
                    }
                    return Result.ToList();
                }
                else
                {
                    IEnumerable<Repository.NewYearRait> Result = new List<NewYearRait>();
                    IEnumerable<Repository.NewYearRait> sequence = from t1 in context.Raitings
                                                                   where t1.Year == year
                                                                   join t2 in context.NewInstitutionsRaitings on t1.Id equals t2.RaitingID
                                                                   join t3 in context.Institutions on t2.InstitutionID equals t3.Id
                                                                   join t4 in context.Locations on t3.LocationID equals t4.Id
                                                                   orderby t2.WorldRank ascending
                                                                   select new Repository.NewYearRait
                                                                   {
                                                                       WordRank = t2.WorldRank,
                                                                       Institution = t3.Name,
                                                                       Location = t4.CountryName,
                                                                       NationalRank = t2.NationalRank,
                                                                       QualityofEducation = t2.QualityofEducation,
                                                                       AlumniEmployment = t2.AlumniEmployment,
                                                                       QualityofFaculty = t2.QualityofFaculty,
                                                                       Publications = t2.Publications,
                                                                       Influence = t2.Influence,
                                                                       Citations = t2.Citations,
                                                                       Patents = t2.Patents,
                                                                       Score = t2.Score,
                                                                       BroadImpact = t2.BroadImpact
                                                                   };
                    if (fl == true)
                    {
                        sequence.ToList();
                        IEnumerable<Repository.NewYearRaitGroup> sequence2 = from t1 in sequence
                                                                             group t1 by t1.Location into g
                                                                             orderby g.Key descending
                                                                             select new Repository.NewYearRaitGroup
                                                                             {
                                                                                 Location = g.Key,
                                                                                 Institutions = g.OrderByDescending(x => x.Score)
                                                                             };

                        foreach (var tec in sequence2)
                        {
                            if (Result.Count() != 0)
                                Result = tec.Institutions.Union(Result);
                            else Result = tec.Institutions;
                        }
                    }
                    else
                    {
                        Result = sequence;
                    }
                    return Result.ToList();
                }
            }
        }
        public static List<Repository.UniversityDynamic> GetDynamic(string UniversityName)
        {
            using (var context = new Context())
            {
                IEnumerable<Repository.UniversityDynamic> sequence = from t1 in context.Institutions
                                                                     where t1.Name == UniversityName
                                                                     join t2 in (context.InstiutionRaitings.Union(context.InstiutionRaitings)) on t1.Id equals t2.InstitutionID
                                                                     join t3 in context.Raitings on t2.RaitingID equals t3.Id
                                                                     select new Repository.UniversityDynamic
                                                                     {
                                                                         Institution = t1.Name,
                                                                         Year = t3.Year,
                                                                         WorldRank = t2.WorldRank,
                                                                         Score = t2.Score
                                                                     };
                 return sequence.ToList();
            }
        }
        public static List<Repository.UniversityScore> GetScore()
        {
            using (var context = new Context())
            {
                IEnumerable<Repository.UniversityScore> sequence = from t1 in context.Institutions
                                                                   join t2 in (context.InstiutionRaitings.Union(context.InstiutionRaitings)) on t1.Id equals t2.InstitutionID
                                                                   join t3 in context.Raitings on t2.RaitingID equals t3.Id
                                                                   group t2 by t2.Institution into g
                                                                   orderby g.Average(x => x.Score) descending
                                                                   select new Repository.UniversityScore
                                                                   {
                                                                       Institution = g.Key.Name,
                                                                       MinScore = g.Min(x => x.Score),
                                                                       MaxScore = g.Max(x => x.Score),
                                                                       AverScore = Math.Round(g.Average(x => x.Score), 2)
                                                                   };

                return sequence.ToList();
            }
        }

    }
}
