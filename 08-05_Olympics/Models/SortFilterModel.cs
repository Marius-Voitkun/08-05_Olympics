using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Models
{
    public class SortFilterModel
    {
        public string SortBy { get; set; }
        public List<string> SortProperties { get; set; } = new() { "Name", "Surname", "Country" };
        public string FilterByCountry { get; set; }
        public string FilterBySport { get; set; }
        public int FilterByTeamActivity { get; set; }

    }
}
