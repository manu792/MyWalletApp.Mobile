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

namespace MyWalletApp.Mobile.Data.Repositories
{
    public class BaseRepository<T> where T : IEntities
    {
        private const string BASE_URL = "http://mywalletappwebapi.azurewebsites.net/api/";

        public string Url
        {
            get
            {
                return BASE_URL;
            }
        }

        public async void Agregar(T obj, string url)
        {
            // Calls Web API to save data
            var client = new HttpClient();
            var uri = new Uri(url);

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                // Success
            }
        }
    }
}