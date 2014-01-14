using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LahpaMobile.Services
{
    public interface IScraperService
    {
        Task<string> GetStringContentsAsync(Uri url);
        Task<byte[]> GetBinaryContent(Uri url);
    }

    public class ScraperService : IScraperService
    {
        public async Task<string> GetStringContentsAsync(Uri url)
        {
            HttpClient httpClient = new HttpClient();
            return await httpClient.GetStringAsync(url);
        }

        public async Task<byte[]> GetBinaryContent(Uri url)
        {
            HttpClient httpClient = new HttpClient();
            return await httpClient.GetByteArrayAsync(url);
        }
    }
}
