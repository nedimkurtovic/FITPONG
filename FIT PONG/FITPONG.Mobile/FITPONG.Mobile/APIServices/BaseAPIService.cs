using FIT_PONG.SharedModels;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.APIServices
{
    public class BaseAPIService
    {
        public string resurs { get; set; }
        //public string APIUrl = $"{Resources.ApiUrl}";
        //public static string Username { get; set; }
        //public static string Password { get; set; }
        public static string Username { get; set; } = "nedim.kurtovic@edu.fit.ba";
        public static string Password { get; set; } = "test123.";
        public static int ID { get; set; } = 20;


#if DEBUG
        protected string APIUrl = "http://localhost:5766/api";
#endif
#if RELEASE
        protected string APIUrl = "http://p1869.app.fit.ba/api";
#endif
        public BaseAPIService(string _resurs)
        {
            resurs = _resurs;
        }
        public async Task<T> GetAll<T>(object searchRequest = null)
        {
            var query = "";
            if (searchRequest != null)
                query = await searchRequest?.ToQueryString();
            var url = $"{APIUrl}/{resurs}";
            if (!String.IsNullOrEmpty(query))
                url += $"?{query}";

            var rezultatApija = await url
                .WithBasicAuth(Username, Password).GetJsonAsync<T>();

            return rezultatApija;
        }
        public async Task<T> GetByID<T>(int id)
        {
            var url = $"{APIUrl}/{resurs}/{id}";
            return await url.WithBasicAuth(Username, Password).GetJsonAsync<T>();
        }

        public async Task<T> Insert<T>(object request)
        {
            var url = $"{APIUrl}/{resurs}";
            try
            {
                return await url.WithBasicAuth(Username, Password)
                    .PostJsonAsync(request).ReceiveJson<T>();

            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
 
                return default(T);
            }
        }
        public virtual async Task<T> Update<T>(int id, object request)
        {
            var url = $"{APIUrl}/{resurs}/{id}";
            try
            {
                return await url.WithBasicAuth(Username, Password).PostJsonAsync(request).ReceiveJson<T>();
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");             
                return default(T);
            }
        }
        public async Task<T> GetAllPaged<T>(string _url)
        {
            if (String.IsNullOrEmpty(_url) || String.IsNullOrWhiteSpace(_url))
                return default(T);
            var url = _url;

            try
            {
                return await url.WithBasicAuth(Username, Password).GetJsonAsync<T>();
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return default(T);
            }
        }
        protected async Task<string> GetErrore(FlurlHttpException ex)
        {
            //var staje = ex.GetResponseJsonAsync();
            //var errori2 = await ex.GetResponseStringAsync();
            //var errori3 = ex.Data;
            
            var errori = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();
            if (errori != null)
            {
                var _stringBilder = new StringBuilder();
                foreach (var i in errori)
                {
                    _stringBilder.AppendLine($"{string.Join("\n", i.Value)}");
                }
                return _stringBilder.ToString();
            }
            return "Greška prilikom povezivanja";
        }
    }
}
