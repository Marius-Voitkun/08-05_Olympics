using _08_05_Olympics.Models;
using _08_05_Olympics.Models.ViewModels;
using _08_05_Olympics.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace _08_05_Olympics.Controllers
{
    public class AthletesController : Controller
    {
        private readonly AthletesIntegratedService _integratedService;
        private readonly AthletesDbService _athletesDbService;

        public AthletesController(AthletesIntegratedService integratedService, AthletesDbService athletesDbService)
        {
            _integratedService = integratedService;
            _athletesDbService = athletesDbService;
        }

        public IActionResult Index()
        {
            IntegratedViewModel model = _integratedService.GetModelForIndex(new SortFilterModel());

            return View(model);
        }

        public IActionResult SortedFilteredIndex(IntegratedViewModel model)
        {
            IntegratedViewModel sortedFilteredModel = _integratedService.GetModelForIndex(model.SortFilter);

            return View("Index", sortedFilteredModel);
        }

        public IActionResult Create()
        {
            IntegratedViewModel model = _integratedService.GetModelForCreate();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(List<AthleteModel> athletes)
        {
            _athletesDbService.AddAthlete(athletes[0]);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            IntegratedViewModel model = _integratedService.GetModelForEdit(id);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(List<AthleteModel> athletes)
        {
            _athletesDbService.UpdateAthlete(athletes[0]);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _athletesDbService.DeleteAthlete(id);

            return RedirectToAction("Index");
        }
    }
}
