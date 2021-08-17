using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Models
{
    public class SportModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool TeamActivity { get; set; }
    }
}
