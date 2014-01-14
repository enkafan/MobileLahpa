using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace LahpaMobile.Services
{    
    public interface IWebPageScheduleParserService
    {
        List<ScheduleLink> ParseLinks(string pageContent);
    }

    public class WebPageScheduleParserService : IWebPageScheduleParserService
    {
        public List<ScheduleLink> ParseLinks(string pageContent)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(pageContent);
            return htmlDocument.DocumentNode.Descendants()
                .Where(i =>i.Name == "a" && i.Attributes["href"] != null && i.Attributes["href"].Value.EndsWith("ics", StringComparison.OrdinalIgnoreCase))
                .Select(htmlNode => new ScheduleLink
                {
                    Title = htmlNode.InnerText,
                    Url = new Uri(htmlNode.Attributes["href"].Value.Replace("webcal://", "http://"))
                }).ToList();
        }
    }



    public class ScheduleLink
    {
        public string Title { get; set; }
        public Uri Url { get; set; }

    }

}