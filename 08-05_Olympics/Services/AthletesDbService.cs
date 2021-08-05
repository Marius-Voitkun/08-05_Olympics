using _08_05_Olympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Services
{
    public class AthletesDbService
    {
        private readonly SqlConnection _connection;

        public AthletesDbService(SqlConnection connection)
        {
            _connection = connection;
        }

        public List<AthleteModel> GetListOfAthletes()
        {
            List<AthleteModel> athletes = new();

            _connection.Open();

            using var command = new SqlCommand("SELECT * FROM dbo.Athletes;", _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                AthleteModel athlete = new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    Country = reader.GetString(3)
                };

                athletes.Add(athlete);
            }

            _connection.Close();

            return athletes;
        }

        public void AddNewAthlete(AthleteModel athlete)
        {
            _connection.Open();

            using var command = new SqlCommand($"INSERT INTO dbo.Athletes (Name, Surname, Country)" +
                $"VALUES ('{athlete.Name}', '{athlete.Surname}', '{athlete.Country}')", _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
