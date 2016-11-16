using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStorageAndProcessing.Data
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; }
    }
}
