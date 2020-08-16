using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
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
            var url = $"{APIUrl}/{resurs}/registracija";
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

        public async Task<SharedModels.Users> PotvrdiMejl(int userId, string token)
        {

            var url = $"{APIUrl}/{resurs}/mail-potvrda?userId={userId}&token={HttpUtility.UrlEncode(token)}";
            try
            {   
                var rezult = await url.PostJsonAsync("").ReceiveJson<SharedModels.Users>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return default(SharedModels.Users);
            }
        }

        public async Task<SharedModels.Users> PosaljiKonfirmacijskiMejl(Email_Password_Request obj)
        {

            var url = $"{APIUrl}/{resurs}/mail";
            try
            {
                var rezult = await url.PostJsonAsync(obj).ReceiveJson<SharedModels.Users>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return default(SharedModels.Users);
            }
        }

        public async Task<SharedModels.Users> PosaljiMailZaPassword(Email_Password_Request obj)
        {

            var url = $"{APIUrl}/{resurs}/password";
            try
            {
                var rezult = await url.PostJsonAsync(obj).ReceiveJson<SharedModels.Users>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return default(SharedModels.Users);
            }
        }

        public async Task<SharedModels.Users> PotvrdiPassword(PasswordPromjena obj)
        {

            var url = $"{APIUrl}/{resurs}/password-potvrda";
            try
            {
                var rezult = await url.PostJsonAsync(obj).ReceiveJson<SharedModels.Users>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return default(SharedModels.Users);
            }
        }

        public async Task<SharedModels.Users> UpdateProfilePicture(int id, SlikaPromjenaRequest obj)
        {

            var url = $"{APIUrl}/{resurs}/{id}/akcije/slika";
            try
            {
                var rezult = await url.WithBasicAuth(Username, Password).PutJsonAsync(obj).ReceiveJson<SharedModels.Users>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return default(SharedModels.Users);
            }
        }

        public async Task<SharedModels.Users> ResetProfilePicture(int id)
        {

            var url = $"{APIUrl}/{resurs}/{id}/akcije/slika";
            try
            {
                var rezult = await url.WithBasicAuth(Username, Password).PostJsonAsync("").ReceiveJson<SharedModels.Users>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return default(SharedModels.Users);
            }
        }

        public async Task<SharedModels.Users> Postovanje(int id)
        {

            var url = $"{APIUrl}/{resurs}/{id}/akcije/postovanje";
            try
            {
                var rezult = await url.WithBasicAuth(Username, Password).PostJsonAsync("").ReceiveJson<SharedModels.Users>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = GetErrore(ex).Result;
                await Application.Current.MainPage.DisplayAlert("Greška", errori, "OK");
                return default(SharedModels.Users);
            }
        }

    }
}
