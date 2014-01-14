using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LahpaMobile.Services;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ScheduleTests
    {
        [Test]
        public async void GetFullSchedule()
        {
            // arrange
            Uri scheduleUri = new Uri("http://lahpa.com/page4/page4.html");

            IGameParser gameParser = new GameParser();
            IScheduleParser scheduleParser = new ScheduleParser(gameParser);
            IWebPageScheduleParserService webPageScheduleParserService = new WebPageScheduleParserService();
            IScraperService scraperService = new ScraperService();
            ScheduleService scheduleService = new ScheduleService(scheduleParser, scraperService, webPageScheduleParserService);

            // act
            List<Schedule> schedule = await scheduleService.GetScheduleAsync(scheduleUri);

            // assert
            Assert.Greater(schedule.Count, 0);
        }
    }
}
