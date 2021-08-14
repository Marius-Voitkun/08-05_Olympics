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

        public IntegratedViewModel GetModelForIndex(SortFilterModel sortFilterModel)
        {
            IntegratedViewModel model = new();
            model.Athletes = _athletesDbService.GetAthletes(sortFilterModel);
            model.Countries = _countriesDbService.GetCountries();
            model.Sports = _sportsDbService.GetSports();
            model.SortFilter = new();

            foreach (var athlete in model.Athletes)
            {
                athlete.Country = model.Countries.SingleOrDefault(c => c.Id == athlete.CountryId);

                List<int> sportsIds = _athletesDbService.GetSportsIdsForAthlete(athlete.Id);

                foreach (int sportId in sportsIds)
                {
                    athlete.Sports.Add(sportId, true);
                }
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

        public IntegratedViewModel GetModelForEdit(int athleteId)
        {
            IntegratedViewModel model = new();

            List<AthleteModel> athletesFromDb = _athletesDbService.GetAthletes(new SortFilterModel());
            AthleteModel athleteForEditing = athletesFromDb.SingleOrDefault(a => a.Id == athleteId);

            model.Athletes = new List<AthleteModel> { athleteForEditing };

            model.Countries = _countriesDbService.GetCountries();
            model.Sports = _sportsDbService.GetSports();

            List<int> sportsIds = _athletesDbService.GetSportsIdsForAthlete(athleteId);

            foreach (var sport in model.Sports)
            {
                if (sportsIds.Contains(sport.Id))
                    athleteForEditing.Sports.Add(sport.Id, true);
                else
                    athleteForEditing.Sports.Add(sport.Id, false);
            }

            return model;
        }
    }
}
