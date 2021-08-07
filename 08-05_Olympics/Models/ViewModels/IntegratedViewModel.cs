using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Models.ViewModels
{
    public class IntegratedViewModel
    {
        public List<AthleteModel> Athletes { get; set; }
        public List<CountryModel> Countries { get; set; }
        public List<SportModel> Sports { get; set; }
    }
}
