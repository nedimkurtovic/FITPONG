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
    public partial class frmReportImageViewer : Form
    {
        public Image SlikaBox
        {
            set
            {
                this.pbSlika.Image = value;
                this.pbSlika.Size = value.Size;
            }
        }
        public frmReportImageViewer()
        {
            InitializeComponent();
        }

        private void frmReportImageViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pbSlika.Image != null)
                pbSlika.Dispose();
        }
    }
}
