using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using ClientConvertisseurV1.Models;
using Windows.Web.Http;
using System.Net.Http.Json;

namespace ClientConvertisseurV1.Services
{
    public class WSService : IService
    {
        private System.Net.Http.HttpClient httpClient;

        public WSService(string url)
        {
            httpClient = new System.Net.Http.HttpClient();
            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json") // Fix le typo "application./json"
            );
        }

        public async Task<List<Devise>> GetDevisesAsync(string nomControleur)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<Devise>>(nomControleur);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}