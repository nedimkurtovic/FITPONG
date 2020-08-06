using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using FIT_PONG.WinForms.APIServices;
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
    public partial class frmIgracSuspenzija : Form
    {
        private readonly UsersAPIService usersApiService = new UsersAPIService();
        private readonly APIService apiServiceVrste = new APIService("vrste-suspenzija");
        private List<VrsteSuspenzija> vrste = null;
        private readonly Users user;

        public frmIgracSuspenzija(Users user)
        {
            InitializeComponent();
            this.user = user;
            txtPrikaznoIme.Text = this.user.PrikaznoIme;
            InitComboValues();
        }

        private async void InitComboValues()
        {
            vrste = await apiServiceVrste.GetAll<List<VrsteSuspenzija>>();
            InitCombo();
        }

        private void InitCombo()
        {
            cmbTipSuspenzije.Items.Add(" ");
            foreach (var vrsta in vrste)
                cmbTipSuspenzije.Items.Add(vrsta.Opis);
        }

        private async void btnSuspenduj_Click(object sender, EventArgs e)
        {
            if (!ValidirajSuspenziju()) {
                MessageBox.Show("Molimo unesite validne podatke.", "Greška"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            SuspenzijaRequest request = new SuspenzijaRequest
            {
                VrstaSuspenzije = cmbTipSuspenzije.SelectedItem.ToString(),
                DatumPocetka = dtpPocetak.Value,
                DatumZavrsetka = dtpKraj.Value
            };

            var rezultat = await usersApiService.Suspenzija(this.user.ID, request);
            if(rezultat != default(Users))
            {
                MessageBox.Show("Igrac uspjesno suspendovan.", "Uspjeh"
                    , MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Close();
            }
        }

        private bool ValidirajSuspenziju()
        {
            if (cmbTipSuspenzije.SelectedItem == null)
                return false;
            if (dtpPocetak == null)
                return false;
            if (dtpKraj == null)
                return false;
            return true;
        }
    }
}
