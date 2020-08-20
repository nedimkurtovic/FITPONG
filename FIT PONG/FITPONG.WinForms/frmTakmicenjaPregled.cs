using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Takmicenja;
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
    public partial class frmTakmicenjaPregled : Form
    {
        //servisi
        private APIService apiService = new APIService("takmicenja");
        private APIService apiServiceSistemi = new APIService("sistemi-takmicenja");
        private APIService apiServiceVrste = new APIService("vrste-takmicenja");
        private APIService apiServiceKategorije = new APIService("kategorije-takmicenja");
        //

        //liste
        private PagedResponse<Takmicenja> takmicenja = null;
        private List<SistemiTakmicenja> sistemi = null;
        private List<VrsteTakmicenja> vrste = null;
        private List<KategorijeTakmicenja> kategorije = null;
        //

        public frmTakmicenjaPregled()
        {
            InitializeComponent();
            dgvTakmicenja.AutoGenerateColumns = true;
            InitCombos();
        }

        private async void InitCombos()
        {
            sistemi = await apiServiceSistemi.GetAll<List<SistemiTakmicenja>>();
            vrste = await apiServiceVrste.GetAll<List<VrsteTakmicenja>>();
            kategorije = await apiServiceKategorije.GetAll<List<KategorijeTakmicenja>>();
            InitComboValues();
        }

        private void InitComboValues()
        {
            cmbSistem.Items.Add(" ");
            cmbVrsta.Items.Add(" ");
            cmbKategorija.Items.Add(" ");

            foreach (var sistem in sistemi)
                cmbSistem.Items.Add(sistem.Opis);

            foreach (var vrsta in vrste)
                cmbVrsta.Items.Add(vrsta.Naziv);

            foreach (var kategorija in kategorije)
                cmbKategorija.Items.Add(kategorija.Opis);

        }

        private async void btnFiltriraj_Click(object sender, EventArgs e)
        {

            TakmicenjeSearch obj = new TakmicenjeSearch
            {
                Naziv = txtNaziv.Text,
                Kategorija = cmbKategorija.SelectedItem != null && 
                             cmbKategorija.SelectedItem.ToString() != " " ? 
                             cmbKategorija.SelectedItem.ToString() : null,
                
                Sistem = cmbSistem.SelectedItem != null &&
                         cmbSistem.SelectedItem.ToString() != " "  ? 
                         cmbSistem.SelectedItem.ToString() : null,
                
                Vrsta = cmbVrsta.SelectedItem != null &&
                        cmbVrsta.SelectedItem.ToString() != " " ? 
                        cmbVrsta.SelectedItem.ToString() : null,
            };
            if (!string.IsNullOrEmpty(txtMinimalniELO.Text))
                obj.MinimalniELO = int.Parse(txtMinimalniELO.Text);
            
            takmicenja = await apiService.GetAll<PagedResponse<Takmicenja>>(obj);
            PrikaziPodatke();
        }

        private void PrikaziPodatke()
        {
            dgvTakmicenja.DataSource = takmicenja.Stavke.Select(x => new TakmicenjeObjekat
            {
                Id = x.ID,
                Naziv=x.Naziv,
                MinELO=x.MinimalniELO,
                Sistem=x.Sistem,
                Vrsta=x.Vrsta,
                Kategorija=x.Kategorija,
                DatumPocetka=x.DatumPocetka,
                DatumZavrsetka=x.DatumZavrsetka
            }).ToList();
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (takmicenja.IducaStranica != null)
            {
                int pozicija = takmicenja.IducaStranica.ToString().LastIndexOf("/") + 1;
                string resurs = takmicenja.IducaStranica.ToString().Substring(pozicija);
                apiService = new APIService(resurs);
                takmicenja = await apiService.GetAll<PagedResponse<Takmicenja>>();
                PrikaziPodatke();
            }
            RegulisiButtone();
        }

        private async void btnBack_Click(object sender, EventArgs e)
        {
            if (takmicenja.ProslaStranica != null)
            {
                int pozicija = takmicenja.ProslaStranica.ToString().LastIndexOf("/") + 1;
                string resurs = takmicenja.ProslaStranica.ToString().Substring(pozicija);
                apiService = new APIService(resurs);
                takmicenja= await apiService.GetAll<PagedResponse<Takmicenja>>();
                PrikaziPodatke();
            }
            RegulisiButtone();
        }

        private void RegulisiButtone()
        {
            btnBack.Enabled = (takmicenja.ProslaStranica == null)
                ? false : true;
            btnNext.Enabled = (takmicenja.IducaStranica == null)
                ? false : true;
        }

        private void dgvTakmicenja_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
            var item = (TakmicenjeObjekat)dgvTakmicenja.SelectedRows[0].DataBoundItem;
            var takmicenje = takmicenja.Stavke
                .Where(x => x.ID == item.Id).FirstOrDefault();
            frmTakmicenjaDetalji frmTakmicenjaDetalji= new frmTakmicenjaDetalji(takmicenje);
            frmTakmicenjaDetalji.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Kliknite na cijeli red umjesto na kolonu");
            }
        }

    }
}
