using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Objave;
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
    public partial class frmObjavaDodaj : Form
    {
        private readonly APIService _apiServis = new APIService("objave");
        public frmObjavaDodaj()
        {
            InitializeComponent();
        }

        private async void btnDodaj_Click(object sender, EventArgs e)
        {
            ObjaveInsertUpdate obj = new ObjaveInsertUpdate
            {
                Naziv = txtNaslov.Text,
                Content = txtSadrzaj.Text
            };
            if (!ValidirajAdd(obj))
            {
                MessageBox.Show("Morate unijeti podatke", "Greška"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var rezultat = await _apiServis.Insert<Objave>(obj);
            if (rezultat != default(Objave))
            {
                MessageBox.Show("Uspješno dodano", "Uspjeh"
                    , MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Close();
            }
        }
        private bool ValidirajAdd(ObjaveInsertUpdate obj)
        {
            if (String.IsNullOrEmpty(obj.Naziv))
                return false;
            if (String.IsNullOrEmpty(obj.Content))
                return false;
            return true;
        }
    }
}
