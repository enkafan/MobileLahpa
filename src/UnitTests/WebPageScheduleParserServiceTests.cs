using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LahpaMobile.Services;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class WebPageScheduleParserServiceTests
    {
        [Test]
        public void ParsingReturnsScheduleLinks()
        {
            // arrange
            string content = File.ReadAllText("schedule.html");
            WebPageScheduleParserService service = new WebPageScheduleParserService();

            // act
            List<ScheduleLink> scheduleLinks = service.ParseLinks(content);

            // assert
            Assert.AreEqual(7, scheduleLinks.Count);

        }

    }
}
