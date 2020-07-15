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
    public partial class frmMain : Form
    {
        private Form trenutnaChildform = null;
        public frmMain()
        {
            InitializeComponent();
            pnlLijevi.BackColor = Color.FromArgb(0, 173, 239);
        }


        private void OtvoriChildFormu(Form novaforma)
        {
            if (trenutnaChildform != null)
                trenutnaChildform.Close();
            trenutnaChildform = novaforma;
            novaforma.TopLevel = false;
            novaforma.FormBorderStyle = FormBorderStyle.None;
            novaforma.Dock = DockStyle.Fill;
            pnlChildForme.Controls.Add(novaforma);
            pnlChildForme.Tag = novaforma;
            novaforma.Parent = pnlChildForme;
            //novaforma.BringToFront();
            novaforma.Show();
            lblNazivTrenutne.Text = novaforma.Text;
        }

        private void btnReporti_Click(object sender, EventArgs e)
        {
            OtvoriChildFormu(new frmReportiPregled());
        }
        private void btnTakmicenja_Click(object sender, EventArgs e)
        {
            OtvoriChildFormu(new frmTakmicenjaPregled());
        }

        private void btnIgraci_Click(object sender, EventArgs e)
        {
            OtvoriChildFormu(new frmIgraciPregled());
        }

        private void btnObjave_Click(object sender, EventArgs e)
        {
            OtvoriChildFormu(new frmObjavePregled());
        }

        private void btnIzvjestaji_Click(object sender, EventArgs e)
        {
            OtvoriChildFormu(new frmGenerisiIzvjestaj());
        }

        private void btnStatistike_Click(object sender, EventArgs e)
        {
            OtvoriChildFormu(new frmStatistikaStranice());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            lblNazivTrenutne.Text = "Home";
            if (trenutnaChildform != null)
                trenutnaChildform.Close();
            trenutnaChildform = null;
        }

    }
}
