using _08_05_Olympics.Models;
using _08_05_Olympics.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Services
{
    public class AthletesIntegratedService
    {
        private readonly SqlConnection _connection;
        private readonly AthletesDbService _athletesDbService;
        private readonly CountriesDbService _countriesDbService;
        private readonly SportsDbService _sportsDbService;

        public AthletesIntegratedService(SqlConnection connection,
                                         AthletesDbService athletesDbService,
                                         CountriesDbService countriesDbService,
                                         SportsDbService sportsDbService)
        {
            _connection = connection;
            _athletesDbService = athletesDbService;
            _countriesDbService = countriesDbService;
            _sportsDbService = sportsDbService;
        }

        public IntegratedViewModel GetModelForIndex()
        {
            IntegratedViewModel model = new();
            model.Athletes = _athletesDbService.GetAthletes();
            model.Countries = _countriesDbService.GetCountries();

            foreach (var athlete in model.Athletes)
            {
                athlete.Country = model.Countries.SingleOrDefault(c => c.Id == athlete.CountryId);
            }

            return model;
        }

        public IntegratedViewModel GetModelForCreate()
        {
            IntegratedViewModel model = new();
            AthleteModel newAthlete = new();

            model.Athletes = new List<AthleteModel> { newAthlete };

            model.Countries = _countriesDbService.GetCountries();
            model.Sports = _sportsDbService.GetSports();

            foreach (var sport in model.Sports)
            {
                newAthlete.Sports.Add(sport.Id, false);
            }

            return model;
        }
    }
}
