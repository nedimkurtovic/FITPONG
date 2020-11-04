using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIT_PONG.WinForms.APIServices
{
    public class UsersAPIService : APIService
    {
        public UsersAPIService(string resurs = "users") : base(resurs) { }

        public async Task<Users> Suspenzija(int userId, SuspenzijaRequest obj)
        {
            var url = $"{APIUrl}/{resurs}/{userId}/akcije/suspenduj";
            try
            {
                var rezult = await url.WithBasicAuth(Username, Password).PostJsonAsync(obj).ReceiveJson<Users>();

                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = await GetErrore(ex);
                MessageBox.Show(errori, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(Users);
            }
        }
        public async Task<Users> Login(Login obj)
        {
            var url = $"{APIUrl}/{resurs}/login";
            try
            {
                var rezult = await url.PostJsonAsync(obj).ReceiveJson<Users>();
                return rezult;
            }
            catch (FlurlHttpException ex)
            {
                var errori = await GetErrore(ex);
                MessageBox.Show(errori, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(Users);
            }
        }
    }
}
