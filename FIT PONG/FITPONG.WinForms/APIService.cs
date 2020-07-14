﻿using FIT_PONG.WinForms.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIT_PONG.SharedModels;
using Flurl.Http;
using System.Windows.Forms;
using System.Net.Configuration;

namespace FIT_PONG.WinForms
{
    public class APIService
    {
        private string resurs;
        public string APIUrl = $"{Resources.ApiUrl}";
        public static string Username { get; set; }
        public static string Password { get; set; }

        public async Task<T> GetAll<T>(object searchRequest = null)
        {
            var query = "";
            if (searchRequest != null)
                query = await searchRequest?.ToQueryString();

            var rezultatApija = await $"{APIUrl}/{resurs}"
                .WithBasicAuth(Username,Password).GetJsonAsync<T>();

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
                var errori = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();
                var _stringbilder = new StringBuilder();
                foreach(var i in errori)
                {
                    _stringbilder.AppendLine($"{i.Key},{string.Join(",", i.Value)}");
                }
                MessageBox.Show(_stringbilder.ToString(), "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var errori = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();
                var _stringBilder = new StringBuilder();
                foreach(var i in errori)
                {
                    _stringBilder.AppendLine($"{i.Key}, {string.Join(",", i.Value)}");
                }
                MessageBox.Show(_stringBilder.ToString(), "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(T);
            }
        }
    }
}