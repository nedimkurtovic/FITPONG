namespace FIT_PONG.WinForms
{
    partial class frmObjavePregled
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
            this.btnNaredna = new System.Windows.Forms.Button();
            this.btnPrethodna = new System.Windows.Forms.Button();
            this.btnDobavi = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblNaziv = new System.Windows.Forms.Label();
            this.txtNaziv = new System.Windows.Forms.TextBox();
            this.btnDodaj = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNaredna
            // 
            this.btnNaredna.Location = new System.Drawing.Point(371, 402);
            this.btnNaredna.Name = "btnNaredna";
            this.btnNaredna.Size = new System.Drawing.Size(75, 23);
            this.btnNaredna.TabIndex = 11;
            this.btnNaredna.Text = "Naredna";
            this.btnNaredna.UseVisualStyleBackColor = true;
            this.btnNaredna.Click += new System.EventHandler(this.btnNaredna_Click);
            // 
            // btnPrethodna
            // 
            this.btnPrethodna.Location = new System.Drawing.Point(246, 402);
            this.btnPrethodna.Name = "btnPrethodna";
            this.btnPrethodna.Size = new System.Drawing.Size(75, 23);
            this.btnPrethodna.TabIndex = 10;
            this.btnPrethodna.Text = "Prethodna";
            this.btnPrethodna.UseVisualStyleBackColor = true;
            this.btnPrethodna.Click += new System.EventHandler(this.btnPrethodna_Click);
            // 
            // btnDobavi
            // 
            this.btnDobavi.Location = new System.Drawing.Point(490, 39);
            this.btnDobavi.Name = "btnDobavi";
            this.btnDobavi.Size = new System.Drawing.Size(124, 23);
            this.btnDobavi.TabIndex = 9;
            this.btnDobavi.Text = "Dobavi";
            this.btnDobavi.UseVisualStyleBackColor = true;
            this.btnDobavi.Click += new System.EventHandler(this.btnDobavi_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(91, 94);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(523, 292);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // lblNaziv
            // 
            this.lblNaziv.AutoSize = true;
            this.lblNaziv.Location = new System.Drawing.Point(88, 23);
            this.lblNaziv.Name = "lblNaziv";
            this.lblNaziv.Size = new System.Drawing.Size(34, 13);
            this.lblNaziv.TabIndex = 13;
            this.lblNaziv.Text = "Naziv";
            // 
            // txtNaziv
            // 
            this.txtNaziv.Location = new System.Drawing.Point(91, 39);
            this.txtNaziv.Name = "txtNaziv";
            this.txtNaziv.Size = new System.Drawing.Size(369, 20);
            this.txtNaziv.TabIndex = 12;
            // 
            // btnDodaj
            // 
            this.btnDodaj.Location = new System.Drawing.Point(539, 402);
            this.btnDodaj.Name = "btnDodaj";
            this.btnDodaj.Size = new System.Drawing.Size(75, 23);
            this.btnDodaj.TabIndex = 14;
            this.btnDodaj.Text = "Dodaj";
            this.btnDodaj.UseVisualStyleBackColor = true;
            this.btnDodaj.Click += new System.EventHandler(this.btnDodaj_Click);
            // 
            // frmObjavePregled
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDodaj);
            this.Controls.Add(this.lblNaziv);
            this.Controls.Add(this.txtNaziv);
            this.Controls.Add(this.btnNaredna);
            this.Controls.Add(this.btnPrethodna);
            this.Controls.Add(this.btnDobavi);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmObjavePregled";
            this.Text = "frmObjavePregled";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNaredna;
        private System.Windows.Forms.Button btnPrethodna;
        private System.Windows.Forms.Button btnDobavi;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblNaziv;
        private System.Windows.Forms.TextBox txtNaziv;
        private System.Windows.Forms.Button btnDodaj;
    }
}