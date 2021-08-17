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

        public IActionResult Index(string message = "")
        {
            List<SportModel> sports = _dbService.GetSports();

            ViewData["Message"] = message;

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

        public IActionResult Edit(int id)
        {
            SportModel sport = _dbService.GetSports().SingleOrDefault(s => s.Id == id);

            if (sport == null)
                return NotFound();

            return View(sport);
        }

        [HttpPost]
        public IActionResult Edit(SportModel sport)
        {
            _dbService.UpdateSport(sport);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (!_dbService.DeleteSport(id))
                return Json(new { redirectToUrl = Url.Action("Index", new { message = "The sport could not be deleted." }) });

            return Json(new { redirectToUrl = Url.Action("Index") });
        }
    }
}
