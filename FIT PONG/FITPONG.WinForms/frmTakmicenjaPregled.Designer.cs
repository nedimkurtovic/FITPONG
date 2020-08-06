namespace FIT_PONG.WinForms
{
    partial class frmTakmicenjaPregled
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
            this.lblNaziv = new System.Windows.Forms.Label();
            this.txtNaziv = new System.Windows.Forms.TextBox();
            this.txtMinimalniELO = new System.Windows.Forms.TextBox();
            this.lblMinimalniELO = new System.Windows.Forms.Label();
            this.lblSistem = new System.Windows.Forms.Label();
            this.cmbSistem = new System.Windows.Forms.ComboBox();
            this.cmbKategorija = new System.Windows.Forms.ComboBox();
            this.lblKategorija = new System.Windows.Forms.Label();
            this.cmbVrsta = new System.Windows.Forms.ComboBox();
            this.lblVrsta = new System.Windows.Forms.Label();
            this.btnFiltriraj = new System.Windows.Forms.Button();
            this.dgvTakmicenja = new System.Windows.Forms.DataGridView();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTakmicenja)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNaziv
            // 
            this.lblNaziv.AutoSize = true;
            this.lblNaziv.Location = new System.Drawing.Point(9, 35);
            this.lblNaziv.Name = "lblNaziv";
            this.lblNaziv.Size = new System.Drawing.Size(43, 17);
            this.lblNaziv.TabIndex = 0;
            this.lblNaziv.Text = "Naziv";
            // 
            // txtNaziv
            // 
            this.txtNaziv.Location = new System.Drawing.Point(12, 55);
            this.txtNaziv.Name = "txtNaziv";
            this.txtNaziv.Size = new System.Drawing.Size(155, 22);
            this.txtNaziv.TabIndex = 1;
            // 
            // txtMinimalniELO
            // 
            this.txtMinimalniELO.Location = new System.Drawing.Point(189, 55);
            this.txtMinimalniELO.Name = "txtMinimalniELO";
            this.txtMinimalniELO.Size = new System.Drawing.Size(155, 22);
            this.txtMinimalniELO.TabIndex = 3;
            // 
            // lblMinimalniELO
            // 
            this.lblMinimalniELO.AutoSize = true;
            this.lblMinimalniELO.Location = new System.Drawing.Point(186, 35);
            this.lblMinimalniELO.Name = "lblMinimalniELO";
            this.lblMinimalniELO.Size = new System.Drawing.Size(98, 17);
            this.lblMinimalniELO.TabIndex = 2;
            this.lblMinimalniELO.Text = "Minimalni ELO";
            // 
            // lblSistem
            // 
            this.lblSistem.AutoSize = true;
            this.lblSistem.Location = new System.Drawing.Point(365, 35);
            this.lblSistem.Name = "lblSistem";
            this.lblSistem.Size = new System.Drawing.Size(50, 17);
            this.lblSistem.TabIndex = 4;
            this.lblSistem.Text = "Sistem";
            // 
            // cmbSistem
            // 
            this.cmbSistem.FormattingEnabled = true;
            this.cmbSistem.Location = new System.Drawing.Point(368, 55);
            this.cmbSistem.Name = "cmbSistem";
            this.cmbSistem.Size = new System.Drawing.Size(121, 24);
            this.cmbSistem.TabIndex = 5;
            // 
            // cmbKategorija
            // 
            this.cmbKategorija.FormattingEnabled = true;
            this.cmbKategorija.Location = new System.Drawing.Point(519, 55);
            this.cmbKategorija.Name = "cmbKategorija";
            this.cmbKategorija.Size = new System.Drawing.Size(121, 24);
            this.cmbKategorija.TabIndex = 7;
            // 
            // lblKategorija
            // 
            this.lblKategorija.AutoSize = true;
            this.lblKategorija.Location = new System.Drawing.Point(516, 35);
            this.lblKategorija.Name = "lblKategorija";
            this.lblKategorija.Size = new System.Drawing.Size(72, 17);
            this.lblKategorija.TabIndex = 6;
            this.lblKategorija.Text = "Kategorija";
            // 
            // cmbVrsta
            // 
            this.cmbVrsta.FormattingEnabled = true;
            this.cmbVrsta.Location = new System.Drawing.Point(667, 55);
            this.cmbVrsta.Name = "cmbVrsta";
            this.cmbVrsta.Size = new System.Drawing.Size(121, 24);
            this.cmbVrsta.TabIndex = 9;
            // 
            // lblVrsta
            // 
            this.lblVrsta.AutoSize = true;
            this.lblVrsta.Location = new System.Drawing.Point(664, 35);
            this.lblVrsta.Name = "lblVrsta";
            this.lblVrsta.Size = new System.Drawing.Size(41, 17);
            this.lblVrsta.TabIndex = 8;
            this.lblVrsta.Text = "Vrsta";
            // 
            // btnFiltriraj
            // 
            this.btnFiltriraj.Location = new System.Drawing.Point(332, 95);
            this.btnFiltriraj.Name = "btnFiltriraj";
            this.btnFiltriraj.Size = new System.Drawing.Size(130, 32);
            this.btnFiltriraj.TabIndex = 10;
            this.btnFiltriraj.Text = "Filtriraj";
            this.btnFiltriraj.UseVisualStyleBackColor = true;
            this.btnFiltriraj.Click += new System.EventHandler(this.btnFiltriraj_Click);
            // 
            // dgvTakmicenja
            // 
            this.dgvTakmicenja.AllowUserToAddRows = false;
            this.dgvTakmicenja.AllowUserToDeleteRows = false;
            this.dgvTakmicenja.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTakmicenja.Location = new System.Drawing.Point(12, 151);
            this.dgvTakmicenja.Name = "dgvTakmicenja";
            this.dgvTakmicenja.ReadOnly = true;
            this.dgvTakmicenja.RowHeadersWidth = 51;
            this.dgvTakmicenja.RowTemplate.Height = 24;
            this.dgvTakmicenja.Size = new System.Drawing.Size(776, 272);
            this.dgvTakmicenja.TabIndex = 11;
            this.dgvTakmicenja.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTakmicenja_CellDoubleClick);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(186, 447);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(98, 34);
            this.btnBack.TabIndex = 12;
            this.btnBack.Text = "Nazad";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(519, 447);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(98, 34);
            this.btnNext.TabIndex = 13;
            this.btnNext.Text = "Naprijed";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // frmTakmicenjaPregled
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 493);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.dgvTakmicenja);
            this.Controls.Add(this.btnFiltriraj);
            this.Controls.Add(this.cmbVrsta);
            this.Controls.Add(this.lblVrsta);
            this.Controls.Add(this.cmbKategorija);
            this.Controls.Add(this.lblKategorija);
            this.Controls.Add(this.cmbSistem);
            this.Controls.Add(this.lblSistem);
            this.Controls.Add(this.txtMinimalniELO);
            this.Controls.Add(this.lblMinimalniELO);
            this.Controls.Add(this.txtNaziv);
            this.Controls.Add(this.lblNaziv);
            this.Name = "frmTakmicenjaPregled";
            this.Text = "frmTakmicenjaPregled";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTakmicenja)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNaziv;
        private System.Windows.Forms.TextBox txtNaziv;
        private System.Windows.Forms.TextBox txtMinimalniELO;
        private System.Windows.Forms.Label lblMinimalniELO;
        private System.Windows.Forms.Label lblSistem;
        private System.Windows.Forms.ComboBox cmbSistem;
        private System.Windows.Forms.ComboBox cmbKategorija;
        private System.Windows.Forms.Label lblKategorija;
        private System.Windows.Forms.ComboBox cmbVrsta;
        private System.Windows.Forms.Label lblVrsta;
        private System.Windows.Forms.Button btnFiltriraj;
        private System.Windows.Forms.DataGridView dgvTakmicenja;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
    }
}