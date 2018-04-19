using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using Newtonsoft.Json;
using MyWalletApp.Mobile.Models;
using System.Threading.Tasks;

namespace MyWalletApp.Mobile.Data.Repositories
{
    public class BaseRepository<T> where T : IEntities
    {
        private const string BASE_URL = "http://mywalletappwebapi.azurewebsites.net/api/";
        private HttpClient client;

        public BaseRepository()
        {
            client = new HttpClient();
        }

        public string Url
        {
            get
            {
                return BASE_URL;
            }
        }

        public async Task Agregar(T obj, string url)
        {
            // Calls Web API to save data
            var uri = new Uri(BASE_URL + url);

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                // Success
            }
        }

        public async Task<IEnumerable<T>> GetAll(string url)
        {
            // Calls Web API to save data
            var obj = default(IEnumerable<T>);

            var uri = new Uri(BASE_URL + url);

            HttpResponseMessage response = null;

            response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                // Success
                var resultString = await response.Content.ReadAsStringAsync();
                obj = JsonConvert.DeserializeObject<IEnumerable<T>>(resultString);
            }

            return obj;
        }

        public async void SearchById(int id, string url)
        {
            // Calls Web API to save data
            var uri = new Uri($"{BASE_URL + url}/{id}");

            HttpResponseMessage response = null;

            response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                // Success
            }
        }

        public async Task Update(int id, T obj, string url)
        {
            // Calls Web API to save data
            var uri = new Uri($"{BASE_URL + url}/{id}");

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PutAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                // Success
            }
        }

        public async Task Update(string id, T obj, string url)
        {
            // Calls Web API to save data
            var uri = new Uri($"{BASE_URL + url}/{id}");

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PutAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                // Success
            }
        }

        public async Task Delete(int id, string url)
        {
            // Calls Web API to save data
            var uri = new Uri($"{BASE_URL + url}/{id}");

            HttpResponseMessage response = null;

            response = await client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                // Success
            }
        }

        public async Task Delete(string id, string url)
        {
            // Calls Web API to save data
            var uri = new Uri($"{BASE_URL + url}/{id}");

            HttpResponseMessage response = null;

            response = await client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                // Success
            }
        }
    }
}