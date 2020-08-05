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
                frmIzvjestajIgraci izvjestajIgraci = new frmIzvjestajIgraci(txtNaziv.Text);
                izvjestajIgraci.ShowDialog();
            }
            else if (cmbIzvjestaj.SelectedItem.ToString() == "Takmicenja")
            {
                MessageBox.Show("Takmicenja report ...");
            };
        }
    }
}
