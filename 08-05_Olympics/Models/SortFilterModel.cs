using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Models
{
    public class SortFilterModel
    {
        public string SortBy { get; set; }
        public string FilterBy { get; set; }
        public string FilteringValue { get; set; }
        public List<string> Properties { get; set; } = new() { "Name", "Surname", "Country", "Sport", "Sport: Team Activity" };
    }
}
