namespace FIT_PONG.WinForms
{
    partial class frmReportiPregled
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtNaziv = new System.Windows.Forms.TextBox();
            this.lblNaziv = new System.Windows.Forms.Label();
            this.lblDatum = new System.Windows.Forms.Label();
            this.dTPDatum = new System.Windows.Forms.DateTimePicker();
            this.btnDobavi = new System.Windows.Forms.Button();
            this.btnPrethodna = new System.Windows.Forms.Button();
            this.btnNaredna = new System.Windows.Forms.Button();
            this.chkZanemari = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(24, 107);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(523, 292);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // txtNaziv
            // 
            this.txtNaziv.Location = new System.Drawing.Point(24, 45);
            this.txtNaziv.Name = "txtNaziv";
            this.txtNaziv.Size = new System.Drawing.Size(114, 20);
            this.txtNaziv.TabIndex = 1;
            // 
            // lblNaziv
            // 
            this.lblNaziv.AutoSize = true;
            this.lblNaziv.Location = new System.Drawing.Point(24, 26);
            this.lblNaziv.Name = "lblNaziv";
            this.lblNaziv.Size = new System.Drawing.Size(34, 13);
            this.lblNaziv.TabIndex = 2;
            this.lblNaziv.Text = "Naziv";
            // 
            // lblDatum
            // 
            this.lblDatum.AutoSize = true;
            this.lblDatum.Location = new System.Drawing.Point(176, 26);
            this.lblDatum.Name = "lblDatum";
            this.lblDatum.Size = new System.Drawing.Size(38, 13);
            this.lblDatum.TabIndex = 3;
            this.lblDatum.Text = "Datum";
            // 
            // dTPDatum
            // 
            this.dTPDatum.Location = new System.Drawing.Point(179, 44);
            this.dTPDatum.Name = "dTPDatum";
            this.dTPDatum.Size = new System.Drawing.Size(200, 20);
            this.dTPDatum.TabIndex = 4;
            // 
            // btnDobavi
            // 
            this.btnDobavi.Location = new System.Drawing.Point(423, 43);
            this.btnDobavi.Name = "btnDobavi";
            this.btnDobavi.Size = new System.Drawing.Size(124, 23);
            this.btnDobavi.TabIndex = 5;
            this.btnDobavi.Text = "Dobavi";
            this.btnDobavi.UseVisualStyleBackColor = true;
            this.btnDobavi.Click += new System.EventHandler(this.btnDobavi_Click);
            // 
            // btnPrethodna
            // 
            this.btnPrethodna.Location = new System.Drawing.Point(179, 415);
            this.btnPrethodna.Name = "btnPrethodna";
            this.btnPrethodna.Size = new System.Drawing.Size(75, 23);
            this.btnPrethodna.TabIndex = 6;
            this.btnPrethodna.Text = "Prethodna";
            this.btnPrethodna.UseVisualStyleBackColor = true;
            this.btnPrethodna.Click += new System.EventHandler(this.btnPrethodna_Click);
            // 
            // btnNaredna
            // 
            this.btnNaredna.Location = new System.Drawing.Point(304, 415);
            this.btnNaredna.Name = "btnNaredna";
            this.btnNaredna.Size = new System.Drawing.Size(75, 23);
            this.btnNaredna.TabIndex = 7;
            this.btnNaredna.Text = "Naredna";
            this.btnNaredna.UseVisualStyleBackColor = true;
            this.btnNaredna.Click += new System.EventHandler(this.btnNaredna_Click);
            // 
            // chkZanemari
            // 
            this.chkZanemari.AutoSize = true;
            this.chkZanemari.Location = new System.Drawing.Point(275, 71);
            this.chkZanemari.Name = "chkZanemari";
            this.chkZanemari.Size = new System.Drawing.Size(102, 17);
            this.chkZanemari.TabIndex = 8;
            this.chkZanemari.Text = "Zanemari datum";
            this.chkZanemari.UseVisualStyleBackColor = true;
            this.chkZanemari.CheckedChanged += new System.EventHandler(this.chkZanemari_CheckedChanged);
            // 
            // frmReportiPregled
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 450);
            this.Controls.Add(this.chkZanemari);
            this.Controls.Add(this.btnNaredna);
            this.Controls.Add(this.btnPrethodna);
            this.Controls.Add(this.btnDobavi);
            this.Controls.Add(this.dTPDatum);
            this.Controls.Add(this.lblDatum);
            this.Controls.Add(this.lblNaziv);
            this.Controls.Add(this.txtNaziv);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmReportiPregled";
            this.Text = "frmReportiPregled";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtNaziv;
        private System.Windows.Forms.Label lblNaziv;
        private System.Windows.Forms.Label lblDatum;
        private System.Windows.Forms.DateTimePicker dTPDatum;
        private System.Windows.Forms.Button btnDobavi;
        private System.Windows.Forms.Button btnPrethodna;
        private System.Windows.Forms.Button btnNaredna;
        private System.Windows.Forms.CheckBox chkZanemari;
    }
}