using _08_05_Olympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Services
{
    public class CountriesDbService
    {
        private readonly SqlConnection _connection;

        public CountriesDbService(SqlConnection connection)
        {
            _connection = connection;
        }

        public List<CountryModel> GetCountries()
        {
            List<CountryModel> countries = new();

            _connection.Open();

            using var command = new SqlCommand("SELECT * FROM dbo.Countries;", _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                CountryModel country = new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    UNDP = reader.GetString(2)
                };

                countries.Add(country);
            }

            _connection.Close();

            return countries;
        }

        public void AddCountry(CountryModel country)
        {
            string query = @$"INSERT INTO dbo.Countries (Name, UNDP)
                              VALUES ('{country.Name}', '{country.UNDP}');";

            ExecuteSqlQuery(query);
        }

        public void UpdateCountry(CountryModel country)
        {
            string query = $@"UPDATE dbo.Countries
                              SET Name = '{country.Name}', UNDP = '{country.UNDP}'
                              WHERE Id = '{country.Id}';";

            ExecuteSqlQuery(query);
        }

        public void DeleteCountry(int id)
        {
            string query = $@"DELETE FROM dbo.Countries
                              WHERE Id = '{id}';";

            ExecuteSqlQuery(query);
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
