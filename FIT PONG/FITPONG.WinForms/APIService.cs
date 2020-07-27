using FIT_PONG.WinForms.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIT_PONG.SharedModels;
using Flurl.Http;
using System.Windows.Forms;
using System.Net.Configuration;
using System.Diagnostics;

namespace FIT_PONG.WinForms
{
    public class APIService
    {
        public string resurs { get; set; }
        public string APIUrl = $"{Resources.ApiUrl}";
        public static string Username { get; set; }
        public static string Password { get; set; }

        public APIService(string _resurs)
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

            try
            {
                return await url
                .WithBasicAuth(Username, Password).GetJsonAsync<T>();
            }
            catch (FlurlHttpException ex)
            {
                var errori = await GetErrore(ex);
                MessageBox.Show(errori, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(T);
            }
        }
        public async Task<T> GetByID<T>(int id)
        {
            var url = $"{APIUrl}/{resurs}/{id}";
            try
            {
                return await url.WithBasicAuth(Username, Password).GetJsonAsync<T>();
            }
            catch (FlurlHttpException ex)
            {
                var errori = await GetErrore(ex);
                MessageBox.Show(errori, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(T);
            }
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
                var errori = await GetErrore(ex);
                MessageBox.Show(errori, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(T);
            }
        }
        public async Task<T> Update<T>(int id, object request)
        {
            var url = $"{APIUrl}/{resurs}/{id}";
            try
            {
                return await url.WithBasicAuth(Username, Password).PostJsonAsync(request).ReceiveJson<T>();
            }
            catch (FlurlHttpException ex)
            {
                var errori = await GetErrore(ex);
                MessageBox.Show(errori, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(T);
            }
        }
        protected async Task<string> GetErrore(FlurlHttpException ex)
        {
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
