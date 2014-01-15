using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDay.iCal;

namespace LahpaMobile.Services
{
    public interface IScheduleParser
    {
        List<SingleGame> ParseCalendar(byte[] calendar, string league);
    }

    public class ScheduleParser : IScheduleParser
    {
        private readonly IGameParser _gameParser;

        public ScheduleParser(IGameParser gameParser)
        {
            _gameParser = gameParser;
        }

        public List<SingleGame> ParseCalendar(byte[] calendar, string league)
        {
            List<SingleGame> games = new List<SingleGame>();
            IICalendarCollection calendars = iCalendar.LoadFromStream(new MemoryStream(calendar));
            foreach (IICalendar item in calendars)
            {
                foreach (IEvent gameEvent in item.Events)
                {

                    games.Add(_gameParser.ParseGame(gameEvent.Summary, gameEvent.Start.Local, gameEvent.Location, league));
                }
            }

            return games;
        }
    }


    public interface IGameParser
    {
        SingleGame ParseGame(string game, DateTime time, string location, string league);
    }

    public class GameParser:IGameParser
    {
        public SingleGame ParseGame(string game, DateTime time, string location, string league)
        {
            // format: "Game {number} {home} vs {away}
            const string regexPattern = @"^Game (?<GameNumber>.*?) (?<HomeTeam>.(?:(?!vs).)*) vs (?<AwayTeam>.*)";
            Regex myRegex = new Regex(regexPattern, RegexOptions.None);


            Match match= myRegex.Match(game);
            if (match.Success == false)
            {
                return new SingleGame() {AwayTeam = "(unknwon)", HomeTeam = "(unknown", Location = location, Time = time};
            }

            return new SingleGame
            {
                HomeTeam = match.Groups["HomeTeam"].Value,
                AwayTeam = match.Groups["AwayTeam"].Value,
                Location = location,
                Time = time,
                League = league
            };
            

        }
    }

    public class SingleGame
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Location { get; set; }
        public DateTime Time { get; set; }
        public string League { get; set; }
    }
}
