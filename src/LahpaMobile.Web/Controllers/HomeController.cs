using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LahpaMobile.Services;
using LahpaMobile.Web.Services;
using LahpaMobile.Web.ViewModels;

namespace LahpaMobile.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICachedScheduleService _scheduleService;

        public HomeController(ICachedScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public async Task<ActionResult> Index()
        {
            List<Schedule> schedules = await _scheduleService.GetScheduleAsync();

            HomeViewModel model = new HomeViewModel();
            model.Schedules = schedules;

            return View(model);
        }
    }
}