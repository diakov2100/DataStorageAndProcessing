using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStorageAndProcessing.Data
{
    public class Institution
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationID { get; set; }
        [Required]
        public Location Location { get; set; }
    }
}
