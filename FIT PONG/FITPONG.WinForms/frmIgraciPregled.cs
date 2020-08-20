using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Account;
using FIT_PONG.WinForms.PomocniObjekti;
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
    public partial class frmIgraciPregled : Form
    {
        private APIService apiService = new APIService("users");
        private PagedResponse<Users> users = null;

        public frmIgraciPregled()
        {
            InitializeComponent();
            dgvIgraci.AutoGenerateColumns = true;
        }

        private async void btnFiltriraj_Click(object sender, EventArgs e)
        {
            AccountSearchRequest obj = new AccountSearchRequest
            {
                PrikaznoIme = txtPrikaznoIme.Text
            };

            users = await apiService.GetAll<PagedResponse<Users>>(obj);
            PrikaziPodatke();
        }

        private void PrikaziPodatke()
        {
            dgvIgraci.DataSource = users.Stavke.Select(x => new IgraciObjekat
            {
                Id = x.ID,
                PrikaznoIme = x.PrikaznoIme,
                ELO = x.ELO,
                BrojPostovanja = x.BrojPostovanja
            }).ToList();
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (users.IducaStranica != null)
            {
                int pozicija = users.IducaStranica.ToString().LastIndexOf("/") + 1;
                string resurs = users.IducaStranica.ToString().Substring(pozicija);
                apiService = new APIService(resurs);
                users = await apiService.GetAll<PagedResponse<Users>>();
                PrikaziPodatke();
            }
            RegulisiButtone();
        }

        private async void btnBack_Click(object sender, EventArgs e)
        {
            if (users.ProslaStranica != null)
            {
                int pozicija = users.ProslaStranica.ToString().LastIndexOf("/") + 1;
                string resurs = users.ProslaStranica.ToString().Substring(pozicija);
                apiService = new APIService(resurs);
                users = await apiService.GetAll<PagedResponse<Users>>();
                PrikaziPodatke();
            }
            RegulisiButtone();
        }

        private void RegulisiButtone()
        {
            btnBack.Enabled = (users.ProslaStranica == null)
                ? false : true;
            btnNext.Enabled = (users.IducaStranica == null)
                ? false : true;
        }

        private void dgvIgraci_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var item = (IgraciObjekat)dgvIgraci.SelectedRows[0].DataBoundItem;
                var igrac = users.Stavke
                    .Where(x => x.ID == item.Id).FirstOrDefault();
                frmIgracDetalji frmIgracDetalji = new frmIgracDetalji(igrac);
                frmIgracDetalji.ShowDialog();
            }
            catch(Exception)
            {
                MessageBox.Show("Kliknite na cijeli red umjesto na kolonu");
            }
        }
    }
}
