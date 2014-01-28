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


            List<SingleGame> upcomingGames = new List<SingleGame>();
            foreach (Schedule schedule in schedules)
            {
                upcomingGames.AddRange(schedule.Games.Where(game => game.Time > DateTime.Today && game.Time < DateTime.Today.AddDays(14)));
            }

            MultiDayScheduleViewModel model = new MultiDayScheduleViewModel
            {
                Description = "Upcoming Games",
                Days = new List<MultiDayScheduleViewModel.Day>()
            };

            bool setActive = false;
            foreach (var game in upcomingGames.OrderBy(i => i.Time))
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
                        Games = new List<SingleGame> { game }
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

        [ChildActionOnly]
        public  ActionResult Menu()
        {
            List<Schedule> schedule = AsyncHelpers.RunSync<List<Schedule>>(() => _scheduleService.GetScheduleAsync());
            List<string> leagues = schedule.Select(i => i.Description).ToList();
            return PartialView(leagues);
        }
    }
}