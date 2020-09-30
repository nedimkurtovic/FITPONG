using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Objave;
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
    
    public partial class frmObjavePregled : Form
    {
        private APIService _apiServis = new APIService("objave");
        private PagedResponse<Objave> _ObjaveLista = null;
        public frmObjavePregled()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = true;
        }

        private async void btnDobavi_Click(object sender, EventArgs e)
        {
            _apiServis = new APIService("objave");
            ObjaveSearch obj = new ObjaveSearch
            {
                Naziv = txtNaziv.Text
            };
            _ObjaveLista = await _apiServis.GetAll<PagedResponse<Objave>>(obj);
            RegulisiDataSource();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
            var item = (PomocniObjekat) dataGridView1.SelectedRows[0].DataBoundItem;
            var RealObjava = _ObjaveLista.Stavke
                .Where(x => x.ID == item.ID).FirstOrDefault();
            frmObjavaDetalji frmDet = new frmObjavaDetalji(RealObjava);
            frmDet.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Kliknite na cijeli red umjesto na kolonu");
            }
        }
        private async void btnNaredna_Click(object sender, EventArgs e)
        {
            if (_ObjaveLista == null)
                return;
            if (_ObjaveLista.IducaStranica != null)
            {
                int pozicija = _ObjaveLista.IducaStranica.ToString().LastIndexOf("/") + 1;
                string resurs = _ObjaveLista.IducaStranica.ToString().Substring(pozicija);
                _apiServis = new APIService(resurs);
                _ObjaveLista = await _apiServis.GetAll<PagedResponse<Objave>>();
                RegulisiDataSource();

            }
            RegulisiButtone();
        }

        private async void btnPrethodna_Click(object sender, EventArgs e)
        {
            if (_ObjaveLista == null)
                return;
            if (_ObjaveLista.ProslaStranica != null)
            {
                int pozicija = _ObjaveLista.ProslaStranica.ToString().LastIndexOf("/") + 1;
                string resurs = _ObjaveLista.ProslaStranica.ToString().Substring(pozicija);
                _apiServis = new APIService(resurs);
                _ObjaveLista = await _apiServis.GetAll<PagedResponse<Objave>>();
                RegulisiDataSource();
            }
            RegulisiButtone();
        }
        private void RegulisiButtone()
        {
            btnPrethodna.Enabled = _ObjaveLista.ProslaStranica != null;
            btnNaredna.Enabled = _ObjaveLista.IducaStranica != null;
        }
        private void RegulisiDataSource()
        {
            dataGridView1.DataSource = _ObjaveLista.Stavke.Select(x => new PomocniObjekat
            {
                ID = x.ID,
                Naslov = x.Naziv,
                Datum = x.DatumKreiranja
            }).ToList();
            RegulisiButtone();
        }
        private void btnDodaj_Click(object sender, EventArgs e)
        {
            frmObjavaDodaj frm = new frmObjavaDodaj();
            frm.Show();
        }

    }
}
