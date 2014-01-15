using System.Collections.Generic;
using LahpaMobile.Services;

namespace LahpaMobile.Web.ViewModels
{
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