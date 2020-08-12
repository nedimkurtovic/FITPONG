using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.APIServices
{
    public class UsersAPIService:BaseAPIService
    {
        public UsersAPIService(string resurs="users") : base(resurs)
        {

        }

        public async Task<Users> Registracija(AccountInsert obj)
        {
            var url = $"{APIUrl}/{resurs}/register";
            try
            {
                //var jsonString = await obj.ToQueryString();
                var rezult = await url.PostJsonAsync(obj).ReceiveJson<Users>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return default(Users);
            }
        }
        public async Task<Users> Login(Login obj)
        {
            var url = $"{APIUrl}/{resurs}/login";
            try
            {
                //var jsonString = await obj.ToQueryString();
                var rezult = await url.PostJsonAsync(obj).ReceiveJson<Users>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = await GetErrore(ex);
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return default(Users);
            }
        }
        public async Task<List<Statistike>> GetStatistike(int id)
        {
            var url = $"{APIUrl}/{resurs}/{id}/statistike";
            try
            {             
                var rezult = await url.WithBasicAuth(Username, Password)
                    .GetJsonAsync<List<Statistike>>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return new List<Statistike>();
            }
        }

        public async Task<List<SharedModels.Users>> GetRecommended(int id)
        {
            var url = $"{APIUrl}/{resurs}/{id}/recommend";
            try
            {
                var rezult = await url.WithBasicAuth(Username, Password)
                    .GetJsonAsync<List<SharedModels.Users>>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return new List<SharedModels.Users>();
            }
        }
    }
}
