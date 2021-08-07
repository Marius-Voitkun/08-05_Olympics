﻿using _08_05_Olympics.Models;
using _08_05_Olympics.Models.ViewModels;
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
        private readonly AthletesIntegratedService _integratedService;
        private readonly AthletesDbService _athletesDbService;

        public AthletesController(AthletesIntegratedService integratedService, AthletesDbService athletesDbService)
        {
            _integratedService = integratedService;
            _athletesDbService = athletesDbService;
        }

        public IActionResult Index()
        {
            IntegratedViewModel model = _integratedService.GetModelForIndex();

            return View(model);
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
    }
}
