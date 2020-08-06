using FIT_PONG.SharedModels;
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
    public partial class frmTakmicenjaDetalji : Form
    {
        private readonly Takmicenja takmicenje;
        public frmTakmicenjaDetalji(FIT_PONG.SharedModels.Takmicenja takmicenje)
        {
            InitializeComponent();
            this.takmicenje = takmicenje;
            txtNaziv.Text = this.takmicenje.Naziv;
            txtMinimalniELO.Text = this.takmicenje.MinimalniELO.ToString();
            txtSistem.Text = this.takmicenje.Sistem;
            txtKategorija.Text = this.takmicenje.Kategorija;
            txtVrsta.Text = this.takmicenje.Vrsta;
            txtDatumPocetka.Text = this.takmicenje.DatumPocetka.ToString();
            txtDatumZavrsetka.Text = this.takmicenje.DatumZavrsetka.ToString();
        }
    }
}
