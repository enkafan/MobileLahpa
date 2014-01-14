using System;
using System.Collections.Generic;
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
            List<Schedule> schedule = _httpContext.Cache["Schedule"] as List<Schedule>;
            if (schedule != null)
                return await Task.FromResult(schedule);


            schedule = await _scheduleService.GetScheduleAsync(new Uri("http://lahpa.com/page4/page4.html"));

            _httpContext.Cache.Add("Schedule", schedule, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            return schedule;
        }
    }
}