using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LahpaMobile.Services
{
    public interface IScheduleService
    {
        Task<List<Schedule>> GetScheduleAsync(Uri scheduleUri);
    }

    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleParser _scheduleParser;
        private readonly IScraperService _scraperService;
        private readonly IWebPageScheduleParserService _webPageScheduleParserService;

        public ScheduleService(IScheduleParser scheduleParser, IScraperService scraperService, IWebPageScheduleParserService webPageScheduleParserService)
        {            
            _scheduleParser = scheduleParser;
            _scraperService = scraperService;
            _webPageScheduleParserService = webPageScheduleParserService;
        }

        public async Task<List<Schedule>> GetScheduleAsync(Uri scheduleUri)
        {
            string schedulePageContent = await _scraperService.GetStringContentsAsync(scheduleUri);
            List<ScheduleLink> scheduleLinks = _webPageScheduleParserService.ParseLinks(schedulePageContent);

            var tasks = scheduleLinks.Select(ProcessSchedule);
            Schedule[] allSchedules = await Task.WhenAll(tasks);

            List<Schedule> parsedSchedules = new List<Schedule>();
            foreach (Schedule schedule in allSchedules)
            {
                if (schedule.Description.StartsWith("C League Ale"))
                {
                    Schedule aleSchedule = parsedSchedules.FirstOrDefault(i => i.Description.StartsWith("C League Ale"));
                    if (aleSchedule == null)
                    {
                        parsedSchedules.Add(new Schedule
                        {
                            Description = "C League Ale",Games = schedule.Games
                        });

                    }
                    else
                    {
                        aleSchedule.Games.AddRange(schedule.Games);
                    }
                }
                else
                {
                    parsedSchedules.Add(schedule);
                }
            }

            return parsedSchedules;
        }

        private async Task<Schedule> ProcessSchedule(ScheduleLink scheduleLink)
        {
            byte[] calendarBytes = await _scraperService.GetBinaryContent(scheduleLink.Url);
            
            return new Schedule
            {
                Games = _scheduleParser.ParseCalendar(calendarBytes, scheduleLink.Title),
                Description = scheduleLink.Title
            };
        }
    }

    public class Schedule
    {
        public string Description { get; set; }
        public List<SingleGame> Games { get; set; } 
    }
}
