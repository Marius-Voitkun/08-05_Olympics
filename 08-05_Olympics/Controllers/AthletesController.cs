using _08_05_Olympics.Models;
using _08_05_Olympics.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Controllers
{
    public class AthletesController : Controller
    {
        private readonly AthletesDbService _dbService;

        public AthletesController(AthletesDbService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult Index()
        {
            List<AthleteModel> athletes = _dbService.GetListOfAthletes();

            return View(athletes);
        }

        public IActionResult Create()
        {
            AthleteModel newAthlete = new();

            return View(newAthlete);
        }

        [HttpPost]
        public IActionResult Create(AthleteModel athlete)
        {
            _dbService.AddNewAthlete(athlete);

            return RedirectToAction("Index");
        }
    }
}
