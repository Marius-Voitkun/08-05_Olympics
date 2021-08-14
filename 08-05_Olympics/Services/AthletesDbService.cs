using _08_05_Olympics.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace _08_05_Olympics.Services
{
    public class AthletesDbService
    {
        private readonly SqlConnection _connection;

        public AthletesDbService(SqlConnection connection)
        {
            _connection = connection;
        }

        public List<AthleteModel> GetAthletes(SortFilterModel sortFilterModel)
        {
            List<AthleteModel> athletes = new();

            string teamActivityRange = GetTeamActivityRangeForQuery(sortFilterModel);

            string queryFragmentForFilteringBySport = sortFilterModel.FilterBySport != null 
                                                        ? $"AND s.Name = '{sortFilterModel.FilterBySport}'" 
                                                        : "";

            string queryFragmentForFilteringByCountry = sortFilterModel.FilterByCountry != null
                                                        ? $"c.Name = '{sortFilterModel.FilterByCountry}' AND"
                                                        : "";

            string queryFragmentForSorting = sortFilterModel.SortBy != null
                                                        ? $"ORDER BY {sortFilterModel.SortBy} "
                                                        : "";

            string query = @$"
SELECT a.Id, a.Name, a.Surname, a.CountryId
FROM dbo.Athletes a
JOIN dbo.Countries c ON a.CountryId = c.Id
WHERE {queryFragmentForFilteringByCountry} a.Id IN (
	    SELECT AthleteId 
	    FROM dbo.AthletesSportsJunction asj
	    JOIN dbo.Sports s ON asj.SportId = s.Id
	    WHERE s.TeamActivity IN {teamActivityRange} {queryFragmentForFilteringBySport}
	    )
{queryFragmentForSorting}";


            _connection.Open();

            using var command = new SqlCommand(query, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                AthleteModel athlete = new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    CountryId = reader.GetInt32(3)
                };

                athletes.Add(athlete);
            }

            _connection.Close();

            return athletes;
        }

        private string GetTeamActivityRangeForQuery(SortFilterModel model)
        {
            if (model.FilterByTeamActivity == 1)
                return "(1)";

            if (model.FilterByTeamActivity == 2)
                return "(0)";

            return "(0, 1)";
        }

        public void AddAthlete(AthleteModel athlete)
        {
            string query = @$"INSERT INTO dbo.Athletes (Name, Surname, CountryId)
                              VALUES ('{athlete.Name}', '{athlete.Surname}', '{athlete.CountryId}');
                              SELECT CAST(SCOPE_IDENTITY() AS INT);";
            
            _connection.Open();

            using var command = new SqlCommand(query, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                athlete.Id = reader.GetInt32(0);
            }

            _connection.Close();

            if (athlete.Id == 0) return;

            AddAthleteSportJunctions(athlete);
        }

        public void UpdateAthlete(AthleteModel athlete)
        {
            string query = @$"UPDATE dbo.Athletes
                              SET Name = '{athlete.Name}',
                                  Surname = '{athlete.Surname}',
                                  CountryId = {athlete.CountryId}
                              WHERE Id = {athlete.Id};";

            ExecuteSqlQuery(query);

            DeleteAthleteSportJunctions(athlete.Id);
            AddAthleteSportJunctions(athlete);
        }

        public void DeleteAthlete(int id)
        {
            DeleteAthleteSportJunctions(id);

            string query = $"DELETE FROM dbo.Athletes WHERE Id = {id};";

            ExecuteSqlQuery(query);
        }

        private void AddAthleteSportJunctions(AthleteModel athlete)
        {
            var sportsWhereAthleteAttends = athlete.Sports.Where(s => s.Value == true).ToDictionary(s => s.Key, s => s.Value);
            if (sportsWhereAthleteAttends.Count == 0)
                return;

            string queryFragment = "";
            for (var i = 0; i < sportsWhereAthleteAttends.Count; i++)
            {
                queryFragment += $"({athlete.Id}, {sportsWhereAthleteAttends.ElementAt(i).Key}), ";
            }

            queryFragment = queryFragment.Remove(queryFragment.Length - 2);

            string query = $"INSERT INTO dbo.AthletesSportsJunction VALUES {queryFragment};";

            ExecuteSqlQuery(query);
        }

        private void DeleteAthleteSportJunctions(int athleteId)
        {
            string query = $"DELETE FROM dbo.AthletesSportsJunction WHERE AthleteId = {athleteId};";

            ExecuteSqlQuery(query);
        }

        public List<int> GetSportsIdsForAthlete(int athleteId)
        {
            List<int> sportIds = new();

            string query = $"SELECT SportId FROM dbo.AthletesSportsJunction WHERE AthleteId = ${athleteId};";

            _connection.Open();

            using var command = new SqlCommand(query, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                sportIds.Add(reader.GetInt32(0));
            }

            _connection.Close();

            return sportIds;
        }

        private void ExecuteSqlQuery(string query)
        {
            _connection.Open();

            using var command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
