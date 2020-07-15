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
    public partial class frmObjavaDetalji : Form
    {
        private readonly Objave objava;
        public frmObjavaDetalji(FIT_PONG.SharedModels.Objave _objava)
        {
            InitializeComponent();
            objava = _objava;
            txtNaslov.Text = objava.Naziv;
            rtxtSadrzaj.Text = objava.Content;
            dtpDatum.Value = RegulisiDatum(objava.DatumKreiranja);
        }
        public DateTime RegulisiDatum(DateTime trenutniOdObjave)
        {
            var minDatum = new DateTime(1753, 1, 1);
            var maxDatum = new DateTime(9998, 12, 31);
            if (trenutniOdObjave > minDatum && trenutniOdObjave < maxDatum)
                return trenutniOdObjave;
            else if (trenutniOdObjave < minDatum)
                return minDatum;
            else if (trenutniOdObjave > maxDatum)
                return maxDatum;
            return minDatum;
        }
    }
}
