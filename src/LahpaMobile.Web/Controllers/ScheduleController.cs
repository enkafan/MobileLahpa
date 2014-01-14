using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LahpaMobile.Services;
using LahpaMobile.Web.Services;

namespace LahpaMobile.Web.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ICachedScheduleService _scheduleService;

        public ScheduleController(ICachedScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }


        public async Task<ActionResult> Index(string id)
        {
            List<Schedule> schedules = await _scheduleService.GetScheduleAsync();
            Schedule schedule = schedules.FirstOrDefault(i => i.Description.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            if (schedule == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            LeagueScheduleViewModel model = new LeagueScheduleViewModel {Description = id};
            model.Days = new List<LeagueScheduleViewModel.Day>();
            bool setActive = false;
            foreach (var game in schedule.Games.OrderBy(i=>i.Time))
            {
                string dayDescription = game.Time.ToLongDateString();
                LeagueScheduleViewModel.Day day = model.Days.FirstOrDefault(i => i.Description == dayDescription);
                if (day != null)
                {
                    day.Games.Add(game);
                }
                else
                {
                    day = new LeagueScheduleViewModel.Day
                    {
                        Description = dayDescription,
                        Games = new List<SingleGame> {game}
                    };
                    if (setActive == false && game.Time.AddDays(-1) < DateTime.Now)
                    {
                        setActive = true;
                        day.Active = true;
                    }
                    model.Days.Add(day);
                }
            }

            return View(model);
        }
	}

    public class LeagueScheduleViewModel
    {
        public List<Day> Days { get; set; }
        public string Description { get; set; }

        public class Day
        {
            public bool Active { get; set; }
            public string Description { get; set; }
            public List<SingleGame> Games { get; set; }
        }
    }
}