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
            
            MultiDayScheduleViewModel model = new MultiDayScheduleViewModel
            {
                Description = id,
                Days = new List<MultiDayScheduleViewModel.Day>()
            };

            bool setActive = false;
            foreach (var game in schedule.Games.OrderBy(i=>i.Time))
            {
                string dayDescription = game.Time.ToLongDateString();
                MultiDayScheduleViewModel.Day gameDay = model.Days.FirstOrDefault(i => i.Description == dayDescription);
                if (gameDay != null)
                {
                    gameDay.Games.Add(game);
                }
                else
                {
                    gameDay = new MultiDayScheduleViewModel.Day
                    {
                        Description = dayDescription,
                        Games = new List<SingleGame> {game}
                    };
                    if (setActive == false && game.Time.AddDays(-1) < DateTime.Now)
                    {
                        setActive = true;
                        gameDay.Active = true;
                    }
                    model.Days.Add(gameDay);
                }
            }

            return View(model);
        }

        public async Task<ActionResult> TeamSchedule(string id, string teamName)
        {
            List<Schedule> schedules = await _scheduleService.GetScheduleAsync();
            Schedule schedule = schedules.FirstOrDefault(i => i.Description.Equals(id, StringComparison.InvariantCultureIgnoreCase));

            if (schedule == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<SingleGame> teamGames = schedule.Games.Where(
                i =>
                    i.AwayTeam.Equals(teamName, StringComparison.InvariantCultureIgnoreCase) ||
                    i.HomeTeam.Equals(teamName, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (teamGames.Count == 0)
            {
                return RedirectToAction("Index", "Home");   
            }

            TeamScheduleViewModel model = new TeamScheduleViewModel
            {
                TeamName = teamName,
                League = id,
                Games = teamGames
            };

            return View(model);
        }
    }

    public class TeamScheduleViewModel
    {
        public List<SingleGame> Games { get; set; }
        public string League { get; set; }
        public string TeamName { get; set; }
    }
}