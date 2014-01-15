using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using LahpaMobile.Services;

namespace LahpaMobile.Web.Services
{
    public interface ICachedScheduleService
    {
        Task<List<Schedule>> GetScheduleAsync();
    }

    public class CachedScheduleService : ICachedScheduleService
    {
        private readonly IScheduleService _scheduleService;
        private readonly HttpContext _httpContext;

        public CachedScheduleService(IScheduleService scheduleService, HttpContext httpContext)
        {
            _scheduleService = scheduleService;
            _httpContext = httpContext;
        }

        public async Task<List<Schedule>> GetScheduleAsync()
        {
            const string scheduleCacheKey = "Schedule";
            List<Schedule> schedule = _httpContext.Cache[scheduleCacheKey] as List<Schedule>;
            
            if (Debugger.IsAttached == false && schedule != null)
                return await Task.FromResult(schedule);

            string scheduleUrl = ConfigurationManager.AppSettings["ScheduleUrl"];
            if (string.IsNullOrWhiteSpace(scheduleUrl))
                scheduleUrl = "http://lahpa.com/page4/page4.html";

            schedule = await _scheduleService.GetScheduleAsync(new Uri(scheduleUrl));

            _httpContext.Cache.Add(scheduleCacheKey, schedule, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            return schedule;
        }
    }
}