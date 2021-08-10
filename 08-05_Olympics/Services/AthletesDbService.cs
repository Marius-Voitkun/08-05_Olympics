﻿using _08_05_Olympics.Models;
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

        public List<AthleteModel> GetAthletes()
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
                    CountryId = reader.GetInt32(3)
                };

                athletes.Add(athlete);
            }

            _connection.Close();

            return athletes;
        }

        public void AddAthlete(AthleteModel athlete)
        {
            string query = @$"INSERT INTO dbo.Athletes (Name, Surname, CountryId)
                              VALUES ('{athlete.Name}', '{athlete.Surname}', '{athlete.CountryId}');";

            _connection.Open();

            using var command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();

            _connection.Close();

            athlete.Id = GetLastAthleteId();
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

            _connection.Open();

            using var command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();

            _connection.Close();

            DeleteAthleteSportJunctions(athlete.Id);
            AddAthleteSportJunctions(athlete);
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

            _connection.Open();

            using var command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }

        private void DeleteAthleteSportJunctions(int athleteId)
        {
            string query = $"DELETE FROM dbo.AthletesSportsJunction WHERE AthleteId = {athleteId};";

            _connection.Open();

            using var command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }

        private int GetLastAthleteId()
        {
            int id = 0;

            _connection.Open();

            using var command = new SqlCommand($"SELECT TOP 1 Id FROM dbo.Athletes ORDER BY Id DESC;", _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }

            _connection.Close();

            return id;
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
    }
}
