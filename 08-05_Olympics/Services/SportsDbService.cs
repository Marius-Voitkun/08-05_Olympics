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
            string query = @$"INSERT INTO dbo.Sports (Name, TeamActivity)
                              VALUES ('{sport.Name}', '{sport.TeamActivity}');";

            ExecuteSqlQuery(query);
        }

        public void UpdateSport(SportModel sport)
        {
            string query = $@"UPDATE dbo.Sports
                              SET Name = '{sport.Name}', TeamActivity = '{sport.TeamActivity}'
                              WHERE Id = {sport.Id};";

            ExecuteSqlQuery(query);
        }

        public bool DeleteSport(int id)
        {
            string query = $@"DELETE FROM dbo.Sports
                              WHERE Id = '{id}';";

            try
            {
                ExecuteSqlQuery(query);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
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
