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
    public partial class frmIgracDetalji : Form
    {
        private readonly Users user;

        public frmIgracDetalji(FIT_PONG.SharedModels.Users user)
        {
            InitializeComponent();
            this.user = user;
            txtPrikaznoIme.Text = this.user.PrikaznoIme;
            txtELO.Text = this.user.ELO.ToString();
            txtBrojPostovanja.Text = this.user.BrojPostovanja.ToString();
            txtSpol.Text = this.user.Spol.ToString();
            txtVisina.Text = this.user.Visina.ToString();
            txtJacaRuka.Text= this.user.JacaRuka;
            txtGrad.Text = this.user.Grad;
        }

        private void btnSuspenduj_Click(object sender, EventArgs e)
        {
            frmIgracSuspenzija frmIgracSuspenzija = new frmIgracSuspenzija(this.user);
            frmIgracSuspenzija.ShowDialog();
        }
    }
}
