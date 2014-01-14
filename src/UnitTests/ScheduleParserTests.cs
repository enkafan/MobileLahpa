using System.Collections.Generic;
using System.IO;
using LahpaMobile.Services;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ScheduleParserTests
    {
        [Test]
        public void CanParseSchedule()
        {
            // arrange
            byte[] calendarBytes = File.ReadAllBytes("C_Ale_2_iCal.ics");
            IGameParser gameParser = new GameParser();
            ScheduleParser parser = new ScheduleParser(gameParser);

            // act
            List<SingleGame> games = parser.ParseCalendar(calendarBytes);
        }
    }
}