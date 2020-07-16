using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Reports;
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
    public partial class frmReportiPregled : Form
    {
        private APIService _apiServis = new APIService("reports");
        private PagedResponse<Reports> _reportsLista = null;
        public frmReportiPregled()
        {
            InitializeComponent();
            dTPDatum.Value = DateTime.Now;
            dataGridView1.AutoGenerateColumns = true;
        }

        private async void btnDobavi_Click(object sender, EventArgs e)
        {
            ReportsSearch req = new ReportsSearch {
                Naslov = txtNaziv.Text,
                Datum = dTPDatum.Value
            };
            if (chkZanemari.Checked)
                req.Datum = null;
            _reportsLista = await _apiServis.GetAll<PagedResponse<Reports>>(req);
            RegulisiDataSource();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var item = dataGridView1.SelectedRows[0].DataBoundItem as PomocniObjekat;
            var RealReport = _reportsLista.Stavke.Where(x => x.ID == item.ID).FirstOrDefault();
            frmReportDetalji frd = new frmReportDetalji(RealReport);
            frd.ShowDialog();
        }

        private async void btnNaredna_Click(object sender, EventArgs e)
        {
            if(_reportsLista.IducaStranica != null)
            {
                int pozicija = _reportsLista.IducaStranica.ToString().LastIndexOf("/") + 1;
                string resurs = _reportsLista.IducaStranica.ToString().Substring(pozicija);
                _apiServis = new APIService(resurs);
                _reportsLista = await _apiServis.GetAll<PagedResponse<Reports>>();
                RegulisiDataSource();
            }
            RegulisiButtone();
        }

        private async void btnPrethodna_Click(object sender, EventArgs e)
        {
            if (_reportsLista.ProslaStranica != null)
            {
                int pozicija = _reportsLista.ProslaStranica.ToString().LastIndexOf("/") + 1;
                string resurs = _reportsLista.ProslaStranica.ToString().Substring(pozicija);
                _apiServis = new APIService(resurs);
                _reportsLista = await _apiServis.GetAll<PagedResponse<Reports>>();
                RegulisiDataSource();
            }
            RegulisiButtone();
        }
        private void RegulisiButtone()
        {
            if (_reportsLista.ProslaStranica == null)
                btnPrethodna.Enabled = false;
            if (_reportsLista.IducaStranica == null)
                btnNaredna.Enabled = false;
        }
        private void RegulisiDataSource()
        {
            dataGridView1.DataSource = _reportsLista.Stavke.Select(x => new PomocniObjekat
            {
                ID = x.ID,
                Naslov = x.Naslov,
                Datum = x.DatumKreiranja
            }).ToList();
        }

        private void chkZanemari_CheckedChanged(object sender, EventArgs e)
        {
            if (chkZanemari.Checked)
                dTPDatum.Enabled = false;
            else
                dTPDatum.Enabled = true;
        }
    }
}
