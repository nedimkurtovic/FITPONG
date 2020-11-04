using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using FIT_PONG.WinForms.APIServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FIT_PONG.WinForms
{
    public partial class frmLogin : Form
    {
        private UsersAPIService apiServis = new UsersAPIService();
        public bool UspjesnoPrijavljen { get; private set; } = false;
        public frmLogin()
        {
            InitializeComponent();
        }

        private async void btnPrijava_Click(object sender, EventArgs e)
        {
         
            Login obj = new Login()
            {
                RememberMe = false,
                Password = txtPassword.Text,
                UserName = txtEmail.Text
            };

            if (!isModelValid(obj))
            {
                return;
            }

            var rezultat = await apiServis.Login(obj);
            if(rezultat != default(Users))
            {
                MessageBox.Show("Uspješan login");
                APIService.Username = obj.UserName;
                APIService.Password = obj.Password;
                UspjesnoPrijavljen = true;
                this.Close();
            }          
        }
        private bool isModelValid(Login obj)
        {
            List<string> listaErrora = new List<string>();

            if (String.IsNullOrEmpty(obj.UserName) || String.IsNullOrWhiteSpace(obj.UserName))
                listaErrora.Add("Morate unijeti email");
            if (String.IsNullOrEmpty(obj.Password) || String.IsNullOrWhiteSpace(obj.Password))
                listaErrora.Add("Morate unijeti password");

            if (listaErrora.Count == 0)
                return true;

            StringBuilder sb = new StringBuilder();
            foreach (var i in listaErrora)
                sb.AppendLine(i);

            MessageBox.Show("Greška", sb.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }
}
