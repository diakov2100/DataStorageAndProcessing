using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStorageAndProcessing.Data
{
    public class InstitutionRaiting
    {
        [Key]
        public int Id { get; set; }
        public int WordRank { get; set; }
        public int NationalRank { get; set; }
        public int QualityofEducation { get; set; }
        public int AlumniEmployment { get; set; }
        public int QualityofFaculty { get; set; }
        public int Publications { get; set; }
        public int Influence { get; set; }
        public int Citations { get; set; }       
        public int? BroadImpact { get; set; }
        public int Patents { get; set; }
        public double Score { get; set; }
        [Required]
        public Raiting Raiting { get; set; }
        public int RaitingID { get; set; }
        [Required]
        public Institution Institution { get; set; }
        public int InstitutionID { get; set; }
    }
}
