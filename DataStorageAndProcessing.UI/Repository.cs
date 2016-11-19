using System;
using System.Collections.Generic;
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
            public int BroadImpact { get; set; }
            public int Patents { get; set; }
            public double Score { get; set; }
        }
        public class YearRaitGroup
        {
            public string Location { get; set; }
            public IEnumerable<YearRait> Institutions { get; set; }
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
    }
}
