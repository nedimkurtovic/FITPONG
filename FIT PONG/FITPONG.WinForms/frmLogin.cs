using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIT_PONG.WinForms
{
    public partial class frmLogin : Form
    {
        private APIService apiServis = new APIService("users/login");
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
            //mislim da bi lijepo bilo da se napravi custom service ili bar metoda
            //u postojeci da se doda koja se zove Login i vraca bool 
            var rezultat = await apiServis.Insert<Users>(obj);
            if(rezultat != default(Users))
            {
                MessageBox.Show("Uspješan login");
                APIService.Username = obj.UserName;
                APIService.Password = obj.Password;
                UspjesnoPrijavljen = true;
                this.Close();
            }
            //else
            //    MessageBox.Show("Neispravni podaci", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
