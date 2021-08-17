using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Models
{
    public class AthleteModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public int CountryId { get; set; }

        public CountryModel Country { get; set; }

        public Dictionary<int, bool> Sports { get; set; } = new();
    }
}
