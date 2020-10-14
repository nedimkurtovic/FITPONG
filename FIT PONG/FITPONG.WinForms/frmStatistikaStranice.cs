using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
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
    
    public partial class frmStatistikaStranice : Form
    {
        private APIService AktivnostiApiService { get; set; } = new APIService("stranica/aktivnosti");
        private APIService StanjeApiService { get; set; } = new APIService("stranica/stanje");
        private PagedResponse<BrojKorisnikaLogs> Aktivnosti { get; set; }
        private StanjeStranice _StanjeStranice { get; set; }
        public frmStatistikaStranice()
        {
            InitializeComponent();
            txtLogiraniKorisnici.Enabled = false;
            txtMaxLogiranihKorisnika.Enabled = false;
            txtZagusenjeAplikacije.Enabled = false;
        }

        private async void btnDobaviAktivnosti_Click(object sender, EventArgs e)
        {
            var src = new SharedModels.Requests.Aktivnosti.AktivnostiSearch();
            Aktivnosti = await AktivnostiApiService
                .GetAll<PagedResponse<SharedModels.BrojKorisnikaLogs>>(src);
            PostaviChart();
        }

        private async void btnNazad_Click(object sender, EventArgs e)
        {
            //btn nazad se odnosi kao nazad u mjesecima, a api je osmisljen tako da iduca 
            //stranica vodi nazad u mjesec, pa ce btnNazad_click zapravo koristiti
            //Iducu stranicu iako se cini kontra intuitivno
            if (Aktivnosti == null)
                return;
            if(Aktivnosti.IducaStranica != null)
            {
                int pozicija = Aktivnosti.IducaStranica.ToString().LastIndexOf("api/") + 4;
                var resurs = Aktivnosti.IducaStranica.ToString().Substring(pozicija);
                var servis = new APIService(resurs);
                Aktivnosti = await servis.GetAll<PagedResponse<BrojKorisnikaLogs>>();
                PostaviChart();
            }
            RegulisiButtone();
     
        }

        private async void btnNaprijed_Click(object sender, EventArgs e)
        {
            if (Aktivnosti == null)
                return;
            if (Aktivnosti.ProslaStranica != null)
            {
                int pozicija = Aktivnosti.ProslaStranica.ToString().LastIndexOf("api/") + 4;
                var resurs = Aktivnosti.ProslaStranica.ToString().Substring(pozicija);
                var servis = new APIService(resurs);
                Aktivnosti = await servis.GetAll<PagedResponse<BrojKorisnikaLogs>>();
                PostaviChart();
            }
            RegulisiButtone();
        }

        private async void btnOsvjeziStanje_Click(object sender, EventArgs e)
        {
            _StanjeStranice = await StanjeApiService.GetAll<StanjeStranice>();
            PostaviStanjeStranice();

        }
        private void PostaviChart()
        {
            if (Aktivnosti != default(PagedResponse<SharedModels.BrojKorisnikaLogs>))
            {
                chartStatistika.DataSource = Aktivnosti.Stavke
                    .Select(x => new KorisniciHelpKlasa
                    {
                        ID = x.ID,
                        Datum = x.Datum.Day + "/" + x.Datum.Month,
                        BrojKorisnika = x.MaxBrojKorisnika
                    }).ToList();

                chartStatistika.Series["Broj korisnika"].XValueMember = "Datum";
                chartStatistika.Series["Broj korisnika"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                chartStatistika.Series["Broj korisnika"].YValueMembers = "BrojKorisnika";
                chartStatistika.Series["Broj korisnika"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            }
            RegulisiButtone();
        }
        private void RegulisiButtone()
        {
            btnNaprijed.Enabled = Aktivnosti.ProslaStranica != null;
            btnNazad.Enabled = Aktivnosti.IducaStranica != null;
        }
        private void PostaviStanjeStranice()
        {
            if(_StanjeStranice != default(StanjeStranice))
            {
                
                txtLogiraniKorisnici.Text = _StanjeStranice.TrenutnoAktivno.ToString();
                txtMaxLogiranihKorisnika.Text = _StanjeStranice.MaxAktivno.ToString();
                txtZagusenjeAplikacije.Text =
                    (_StanjeStranice.DatumZagusenja != null)
                    ? _StanjeStranice.DatumZagusenja.GetValueOrDefault().Date.ToString()
                    : "Još uvijek se nije desilo";
            }
        }
    }
    public class KorisniciHelpKlasa
    {
        public int ID { get; set; }
        public int BrojKorisnika { get; set; }
        public string Datum { get; set; }
    }
}
