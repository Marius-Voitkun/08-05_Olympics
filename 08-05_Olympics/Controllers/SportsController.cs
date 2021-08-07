using _08_05_Olympics.Models;
using _08_05_Olympics.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Controllers
{
    public class SportsController : Controller
    {
        private readonly SportsDbService _dbService;

        public SportsController(SportsDbService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult Index()
        {
            List<SportModel> sports = _dbService.GetSports();

            return View(sports);
        }

        public IActionResult Create()
        {
            SportModel newSport = new();

            return View(newSport);
        }

        [HttpPost]
        public IActionResult Create(SportModel sport)
        {
            _dbService.AddSport(sport);

            return RedirectToAction("Index");
        }
    }
}
