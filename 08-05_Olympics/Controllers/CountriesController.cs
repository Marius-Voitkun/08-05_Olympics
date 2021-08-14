using _08_05_Olympics.Models;
using _08_05_Olympics.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace _08_05_Olympics.Controllers
{
    public class CountriesController : Controller
    {
        private readonly CountriesDbService _dbService;

        public CountriesController(CountriesDbService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult Index(string message = "")
        {
            List<CountryModel> countries = _dbService.GetCountries();

            ViewData["Message"] = message;

            return View(countries);
        }

        public IActionResult Create()
        {
            CountryModel newCountry = new();

            return View(newCountry);
        }

        [HttpPost]
        public IActionResult Create(CountryModel country)
        {
            _dbService.AddCountry(country);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            CountryModel country = _dbService.GetCountries().SingleOrDefault(c => c.Id == id);

            if (country == null)
                return NotFound();

            return View(country);
        }

        [HttpPost]
        public IActionResult Edit(CountryModel country)
        {
            _dbService.UpdateCountry(country);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (!_dbService.DeleteCountry(id))
                return Json(new { redirectToUrl = Url.Action("Index", new { message = "The country could not be deleted." }) });
                //return RedirectToAction("AfterDelete", "Countries", new { message = "The country could not be deleted." }); - does not work as intended...

            return Json(new { redirectToUrl = Url.Action("Index") });
        }
    }
}
