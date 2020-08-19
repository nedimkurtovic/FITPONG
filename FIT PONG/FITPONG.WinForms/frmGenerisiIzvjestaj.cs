using FIT_PONG.WinForms.Izvjestaji;
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
    public partial class frmGenerisiIzvjestaj : Form
    {
        public frmGenerisiIzvjestaj()
        {
            InitializeComponent();
            InitComboValues();
        }

        private void InitComboValues()
        {
            cmbIzvjestaj.Items.Add("Igraci");
            cmbIzvjestaj.Items.Add("Takmicenja");
        }

        private void btnGenerisiIzvjestaj_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtNaziv.Text) || cmbIzvjestaj.SelectedItem == null)
            {
                MessageBox.Show("Polja su obavezna.");
                return;
            }

            if (cmbIzvjestaj.SelectedItem.ToString() == "Igraci")
            {
                frmIzvjestajIgraci izvjestajIgraci = new frmIzvjestajIgraci(txtNaziv.Text, GetSelektovaneIgraci());
                izvjestajIgraci.ShowDialog();
            }
            else if (cmbIzvjestaj.SelectedItem.ToString() == "Takmicenja")
            {
                frmIzvjestajTakmicenja izvjestajTakmicenja = new frmIzvjestajTakmicenja(txtNaziv.Text,GetSelektovaneTakmicenja());
                izvjestajTakmicenja.ShowDialog();
            };
        }

        private List<string> GetSelektovaneIgraci()
        {

            var selektovani = new List<string>();
            if (chbJacaruka.Checked)
                selektovani.Add("jacaruka");
            if (chbVisina.Checked)
                selektovani.Add("visina");
            if (chbSpol.Checked)
                selektovani.Add("spol");
            if (chbELO.Checked)
                selektovani.Add("elo");
            if (chbBrojPosjeta.Checked)
                selektovani.Add("brojposjeta");
            return selektovani;
        }

        private List<string> GetSelektovaneTakmicenja()
        {

            var selektovani = new List<string>();
            if (chbBrojRundi.Checked)
                selektovani.Add("brojrundi");
            if (chbMinELO.Checked)
                selektovani.Add("minelo");
            return selektovani;
        }

        private void cmbIzvjestaj_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIzvjestaj.SelectedItem.ToString() == "Igraci")
            {
                flwIgraci.Visible = true;
                flwTakmicenja.Visible = false;
            }

            if (cmbIzvjestaj.SelectedItem.ToString() == "Takmicenja")
            {
                flwIgraci.Visible = false;
                flwTakmicenja.Visible = true;
            }

        }
    }
}
