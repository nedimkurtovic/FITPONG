namespace FIT_PONG.WinForms
{
    partial class frmGenerisiIzvjestaj
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
            this.cmbKategorija = new System.Windows.Forms.ComboBox();
            this.lblKategorija = new System.Windows.Forms.Label();
            this.lblIzvjestaj = new System.Windows.Forms.Label();
            this.cmbIzvjestaj = new System.Windows.Forms.ComboBox();
            this.lblNaziv = new System.Windows.Forms.Label();
            this.txtNaziv = new System.Windows.Forms.TextBox();
            this.txtBrojZapisaPoStranici = new System.Windows.Forms.TextBox();
            this.lblBrojZapisaPoStranici = new System.Windows.Forms.Label();
            this.lblPolja = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbKategorija
            // 
            this.cmbKategorija.FormattingEnabled = true;
            this.cmbKategorija.Location = new System.Drawing.Point(38, 52);
            this.cmbKategorija.Name = "cmbKategorija";
            this.cmbKategorija.Size = new System.Drawing.Size(173, 24);
            this.cmbKategorija.TabIndex = 0;
            // 
            // lblKategorija
            // 
            this.lblKategorija.AutoSize = true;
            this.lblKategorija.Location = new System.Drawing.Point(35, 32);
            this.lblKategorija.Name = "lblKategorija";
            this.lblKategorija.Size = new System.Drawing.Size(137, 17);
            this.lblKategorija.TabIndex = 1;
            this.lblKategorija.Text = "Odaberite kategoriju";
            // 
            // lblIzvjestaj
            // 
            this.lblIzvjestaj.AutoSize = true;
            this.lblIzvjestaj.Location = new System.Drawing.Point(35, 104);
            this.lblIzvjestaj.Name = "lblIzvjestaj";
            this.lblIzvjestaj.Size = new System.Drawing.Size(116, 17);
            this.lblIzvjestaj.TabIndex = 3;
            this.lblIzvjestaj.Text = "Izaberite izvjestaj";
            // 
            // cmbIzvjestaj
            // 
            this.cmbIzvjestaj.FormattingEnabled = true;
            this.cmbIzvjestaj.Location = new System.Drawing.Point(38, 124);
            this.cmbIzvjestaj.Name = "cmbIzvjestaj";
            this.cmbIzvjestaj.Size = new System.Drawing.Size(173, 24);
            this.cmbIzvjestaj.TabIndex = 2;
            // 
            // lblNaziv
            // 
            this.lblNaziv.AutoSize = true;
            this.lblNaziv.Location = new System.Drawing.Point(349, 32);
            this.lblNaziv.Name = "lblNaziv";
            this.lblNaziv.Size = new System.Drawing.Size(43, 17);
            this.lblNaziv.TabIndex = 4;
            this.lblNaziv.Text = "Naziv";
            // 
            // txtNaziv
            // 
            this.txtNaziv.Location = new System.Drawing.Point(352, 54);
            this.txtNaziv.Name = "txtNaziv";
            this.txtNaziv.Size = new System.Drawing.Size(185, 22);
            this.txtNaziv.TabIndex = 5;
            // 
            // txtBrojZapisaPoStranici
            // 
            this.txtBrojZapisaPoStranici.Location = new System.Drawing.Point(352, 124);
            this.txtBrojZapisaPoStranici.Name = "txtBrojZapisaPoStranici";
            this.txtBrojZapisaPoStranici.Size = new System.Drawing.Size(185, 22);
            this.txtBrojZapisaPoStranici.TabIndex = 7;
            // 
            // lblBrojZapisaPoStranici
            // 
            this.lblBrojZapisaPoStranici.AutoSize = true;
            this.lblBrojZapisaPoStranici.Location = new System.Drawing.Point(349, 102);
            this.lblBrojZapisaPoStranici.Name = "lblBrojZapisaPoStranici";
            this.lblBrojZapisaPoStranici.Size = new System.Drawing.Size(147, 17);
            this.lblBrojZapisaPoStranici.TabIndex = 6;
            this.lblBrojZapisaPoStranici.Text = "Broj zapisa po stranici";
            // 
            // lblPolja
            // 
            this.lblPolja.AutoSize = true;
            this.lblPolja.Location = new System.Drawing.Point(35, 199);
            this.lblPolja.Name = "lblPolja";
            this.lblPolja.Size = new System.Drawing.Size(105, 17);
            this.lblPolja.TabIndex = 8;
            this.lblPolja.Text = "Odaberite polja";
            // 
            // frmGenerisiIzvjestaj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 450);
            this.Controls.Add(this.lblPolja);
            this.Controls.Add(this.txtBrojZapisaPoStranici);
            this.Controls.Add(this.lblBrojZapisaPoStranici);
            this.Controls.Add(this.txtNaziv);
            this.Controls.Add(this.lblNaziv);
            this.Controls.Add(this.lblIzvjestaj);
            this.Controls.Add(this.cmbIzvjestaj);
            this.Controls.Add(this.lblKategorija);
            this.Controls.Add(this.cmbKategorija);
            this.Name = "frmGenerisiIzvjestaj";
            this.Text = "frmGenerisiIzvjestaj";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbKategorija;
        private System.Windows.Forms.Label lblKategorija;
        private System.Windows.Forms.Label lblIzvjestaj;
        private System.Windows.Forms.ComboBox cmbIzvjestaj;
        private System.Windows.Forms.Label lblNaziv;
        private System.Windows.Forms.TextBox txtNaziv;
        private System.Windows.Forms.TextBox txtBrojZapisaPoStranici;
        private System.Windows.Forms.Label lblBrojZapisaPoStranici;
        private System.Windows.Forms.Label lblPolja;
    }
}