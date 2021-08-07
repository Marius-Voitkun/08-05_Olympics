using _08_05_Olympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Services
{
    public class SportsDbService
    {
        private readonly SqlConnection _connection;

        public SportsDbService(SqlConnection connection)
        {
            _connection = connection;
        }

        public List<SportModel> GetSports()
        {
            List<SportModel> sports = new();

            _connection.Open();

            using var command = new SqlCommand("SELECT * FROM dbo.Sports;", _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                SportModel sport = new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    TeamActivity = reader.GetBoolean(2)
                };

                sports.Add(sport);
            }

            _connection.Close();

            return sports;
        }

        public void AddSport(SportModel sport)
        {
            _connection.Open();

            using var command = new SqlCommand($"INSERT INTO dbo.Sports (Name, TeamActivity)" +
                $"VALUES ('{sport.Name}', '{sport.TeamActivity}');", _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
