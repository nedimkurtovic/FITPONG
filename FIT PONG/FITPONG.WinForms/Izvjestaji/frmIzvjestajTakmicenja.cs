using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIT_PONG.WinForms.Izvjestaji
{
    public partial class frmIzvjestajTakmicenja : Form
    {
        APIService apiService = new APIService("takmicenja");
        private string naziv;
        private List<string> selektovani;

        private PagedResponse<Takmicenja> takmicenja = null;

        public frmIzvjestajTakmicenja(string naziv, List<string> selektovani)
        {
            InitializeComponent();
            this.naziv = naziv;
            this.selektovani = selektovani;
        }

        private async void frmIzvjestajTakmicenja_Load(object sender, EventArgs e)
        {
            ReportParameterCollection rpc = new ReportParameterCollection();
            rpc.Add(new ReportParameter("Datum", DateTime.Now.ToString()));
            rpc.Add(new ReportParameter("Naziv", naziv));
            rpc.Add(new ReportParameter("brojRundi_visible", selektovani.Contains("brojrundi") ? "True" : "False"));
            rpc.Add(new ReportParameter("MinElo_visible", selektovani.Contains("minelo") ? "True" : "False"));


            takmicenja = await apiService.GetAll<PagedResponse<Takmicenja>>();

            DSTakmicenja.TakmicenjaDataTable tbl = new DSTakmicenja.TakmicenjaDataTable();

            do
            {
                foreach (var t in takmicenja.Stavke)
                {
                    DSTakmicenja.TakmicenjaRow red = tbl.NewTakmicenjaRow();
                    red.ID = t.ID;
                    red.BrojRundi = t.BrojRundi;
                    red.DatumPocetka = t.DatumPocetka??default(DateTime);
                    red.DatumZavrsetka= t.DatumZavrsetka??default(DateTime);
                    red.MinimalniELO= t.MinimalniELO;
                    red.Naziv = t.Naziv;
                    red.RokPocetkaPrijave = t.DatumPocetkaPrijava?? default(DateTime);
                    red.RokZavrsetkaPrijave = t.DatumZavrsetkaPrijava?? default(DateTime);
                    red.DatumKreiranja = t.DatumKreiranja;

                    tbl.Rows.Add(red);
                }

                if (takmicenja.IducaStranica != null)
                {
                    int pozicija = takmicenja.IducaStranica.ToString().LastIndexOf("/") + 1;
                    string resurs = takmicenja.IducaStranica.ToString().Substring(pozicija);
                    apiService = new APIService(resurs);
                    takmicenja = await apiService.GetAll<PagedResponse<Takmicenja>>();
                }
                else
                {
                    takmicenja = null;
                }

            } while (takmicenja != null);


            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DSTakmicenja";
            rds.Value = tbl;


            rpvTakmicenja.LocalReport.ReportPath = "Izvjestaji/rptTakmicenja.rdlc";
            rpvTakmicenja.LocalReport.SetParameters(rpc);
            rpvTakmicenja.LocalReport.DataSources.Add(rds);

            this.rpvTakmicenja.RefreshReport();
        }

    }
}
