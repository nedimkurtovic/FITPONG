using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIT_PONG.WinForms
{
    public partial class frmReportDetalji : Form
    {
        List<string> ImenaFajlova = new List<string>();
        public frmReportDetalji()
        {

            //TODOS
            //[]GEtbyid na api servis?
            //[]prikazati osnovne info
            //[]ucitati slike na listview
            //[]dodati event na itemactive na pojedinacnu stavku listviewa
            
            InitializeComponent();
        }

        private void btnPriloziTest_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() 
            { 
                Multiselect = true,ValidateNames=true,Filter="JPEG|*.jpg|PNG|*.png"
            })
            {
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    ImenaFajlova.Clear();
                    listView1.Items.Clear();
                    int index = 0;
                    foreach(string i in ofd.FileNames)
                    {
                        //RADIII OLEOLE010310ELDALSD
                        //Todos:
                        //prilagoditi tako da se koristi memory stream od niza bajtova koji ce se primiti sa apija
                        //ova funkcija nece vise postojati, nema vise showdialog ali ovaj dio koda gdje se puni 
                        //Listviewimtems i imagelist ce morati postojati!
                        FileInfo x = new FileInfo(i);
                        ImenaFajlova.Add(x.FullName);
                        listView1.Items.Add(x.Name,index++);
                        Image slikica = Image.FromFile(x.FullName);
                        imageList1.Images.Add(slikica);
                        //listView1.SmallImageList = new ImageList();
                        //listView1.SmallImageList.Images.Add(slikica);
                    }
                }
            }
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if(listView1.FocusedItem != null)
            {
                using(frmReportImageViewer x = new frmReportImageViewer())
                {
                    Image img = Image.FromFile(ImenaFajlova[listView1.FocusedItem.Index]);
                    x.SlikaBox = img;
                    x.ShowDialog();
                }
            }
        }
    }
}
