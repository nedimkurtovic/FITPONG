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
            this.lblIzvjestaj = new System.Windows.Forms.Label();
            this.cmbIzvjestaj = new System.Windows.Forms.ComboBox();
            this.lblNaziv = new System.Windows.Forms.Label();
            this.txtNaziv = new System.Windows.Forms.TextBox();
            this.btnGenerisiIzvjestaj = new System.Windows.Forms.Button();
            this.chbJacaruka = new System.Windows.Forms.CheckBox();
            this.chbVisina = new System.Windows.Forms.CheckBox();
            this.chbELO = new System.Windows.Forms.CheckBox();
            this.chbSpol = new System.Windows.Forms.CheckBox();
            this.chbBrojPosjeta = new System.Windows.Forms.CheckBox();
            this.flwIgraci = new System.Windows.Forms.FlowLayoutPanel();
            this.flwTakmicenja = new System.Windows.Forms.FlowLayoutPanel();
            this.chbBrojRundi = new System.Windows.Forms.CheckBox();
            this.chbMinELO = new System.Windows.Forms.CheckBox();
            this.flwIgraci.SuspendLayout();
            this.flwTakmicenja.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblIzvjestaj
            // 
            this.lblIzvjestaj.AutoSize = true;
            this.lblIzvjestaj.Location = new System.Drawing.Point(30, 34);
            this.lblIzvjestaj.Name = "lblIzvjestaj";
            this.lblIzvjestaj.Size = new System.Drawing.Size(116, 17);
            this.lblIzvjestaj.TabIndex = 3;
            this.lblIzvjestaj.Text = "Izaberite izvjestaj";
            // 
            // cmbIzvjestaj
            // 
            this.cmbIzvjestaj.FormattingEnabled = true;
            this.cmbIzvjestaj.Location = new System.Drawing.Point(33, 54);
            this.cmbIzvjestaj.Name = "cmbIzvjestaj";
            this.cmbIzvjestaj.Size = new System.Drawing.Size(173, 24);
            this.cmbIzvjestaj.TabIndex = 2;
            this.cmbIzvjestaj.SelectedIndexChanged += new System.EventHandler(this.cmbIzvjestaj_SelectedIndexChanged);
            // 
            // lblNaziv
            // 
            this.lblNaziv.AutoSize = true;
            this.lblNaziv.Location = new System.Drawing.Point(404, 34);
            this.lblNaziv.Name = "lblNaziv";
            this.lblNaziv.Size = new System.Drawing.Size(43, 17);
            this.lblNaziv.TabIndex = 4;
            this.lblNaziv.Text = "Naziv";
            // 
            // txtNaziv
            // 
            this.txtNaziv.Location = new System.Drawing.Point(407, 56);
            this.txtNaziv.Name = "txtNaziv";
            this.txtNaziv.Size = new System.Drawing.Size(185, 22);
            this.txtNaziv.TabIndex = 5;
            // 
            // btnGenerisiIzvjestaj
            // 
            this.btnGenerisiIzvjestaj.Location = new System.Drawing.Point(239, 323);
            this.btnGenerisiIzvjestaj.Name = "btnGenerisiIzvjestaj";
            this.btnGenerisiIzvjestaj.Size = new System.Drawing.Size(177, 40);
            this.btnGenerisiIzvjestaj.TabIndex = 6;
            this.btnGenerisiIzvjestaj.Text = "Generisi izvjestaj";
            this.btnGenerisiIzvjestaj.UseVisualStyleBackColor = true;
            this.btnGenerisiIzvjestaj.Click += new System.EventHandler(this.btnGenerisiIzvjestaj_Click);
            // 
            // chbJacaruka
            // 
            this.chbJacaruka.AutoSize = true;
            this.chbJacaruka.Location = new System.Drawing.Point(3, 57);
            this.chbJacaruka.Name = "chbJacaruka";
            this.chbJacaruka.Size = new System.Drawing.Size(92, 21);
            this.chbJacaruka.TabIndex = 7;
            this.chbJacaruka.Text = "Jaca ruka";
            this.chbJacaruka.UseVisualStyleBackColor = true;
            // 
            // chbVisina
            // 
            this.chbVisina.AutoSize = true;
            this.chbVisina.Location = new System.Drawing.Point(3, 3);
            this.chbVisina.Name = "chbVisina";
            this.chbVisina.Size = new System.Drawing.Size(68, 21);
            this.chbVisina.TabIndex = 8;
            this.chbVisina.Text = "Visina";
            this.chbVisina.UseVisualStyleBackColor = true;
            // 
            // chbELO
            // 
            this.chbELO.AutoSize = true;
            this.chbELO.Location = new System.Drawing.Point(3, 111);
            this.chbELO.Name = "chbELO";
            this.chbELO.Size = new System.Drawing.Size(58, 21);
            this.chbELO.TabIndex = 9;
            this.chbELO.Text = "ELO";
            this.chbELO.UseVisualStyleBackColor = true;
            // 
            // chbSpol
            // 
            this.chbSpol.AutoSize = true;
            this.chbSpol.Location = new System.Drawing.Point(3, 84);
            this.chbSpol.Name = "chbSpol";
            this.chbSpol.Size = new System.Drawing.Size(58, 21);
            this.chbSpol.TabIndex = 10;
            this.chbSpol.Text = "Spol";
            this.chbSpol.UseVisualStyleBackColor = true;
            // 
            // chbBrojPosjeta
            // 
            this.chbBrojPosjeta.AutoSize = true;
            this.chbBrojPosjeta.Location = new System.Drawing.Point(3, 30);
            this.chbBrojPosjeta.Name = "chbBrojPosjeta";
            this.chbBrojPosjeta.Size = new System.Drawing.Size(105, 21);
            this.chbBrojPosjeta.TabIndex = 11;
            this.chbBrojPosjeta.Text = "Broj posjeta";
            this.chbBrojPosjeta.UseVisualStyleBackColor = true;
            // 
            // flwIgraci
            // 
            this.flwIgraci.Controls.Add(this.chbVisina);
            this.flwIgraci.Controls.Add(this.chbBrojPosjeta);
            this.flwIgraci.Controls.Add(this.chbJacaruka);
            this.flwIgraci.Controls.Add(this.chbSpol);
            this.flwIgraci.Controls.Add(this.chbELO);
            this.flwIgraci.Location = new System.Drawing.Point(33, 123);
            this.flwIgraci.Name = "flwIgraci";
            this.flwIgraci.Size = new System.Drawing.Size(113, 143);
            this.flwIgraci.TabIndex = 12;
            this.flwIgraci.Visible = false;
            // 
            // flwTakmicenja
            // 
            this.flwTakmicenja.Controls.Add(this.chbBrojRundi);
            this.flwTakmicenja.Controls.Add(this.chbMinELO);
            this.flwTakmicenja.Location = new System.Drawing.Point(164, 123);
            this.flwTakmicenja.Name = "flwTakmicenja";
            this.flwTakmicenja.Size = new System.Drawing.Size(127, 68);
            this.flwTakmicenja.TabIndex = 13;
            this.flwTakmicenja.Visible = false;
            // 
            // chbBrojRundi
            // 
            this.chbBrojRundi.AutoSize = true;
            this.chbBrojRundi.Location = new System.Drawing.Point(3, 3);
            this.chbBrojRundi.Name = "chbBrojRundi";
            this.chbBrojRundi.Size = new System.Drawing.Size(91, 21);
            this.chbBrojRundi.TabIndex = 8;
            this.chbBrojRundi.Text = "Broj rundi";
            this.chbBrojRundi.UseVisualStyleBackColor = true;
            // 
            // chbMinELO
            // 
            this.chbMinELO.AutoSize = true;
            this.chbMinELO.Location = new System.Drawing.Point(3, 30);
            this.chbMinELO.Name = "chbMinELO";
            this.chbMinELO.Size = new System.Drawing.Size(120, 21);
            this.chbMinELO.TabIndex = 11;
            this.chbMinELO.Text = "Minimalni ELO";
            this.chbMinELO.UseVisualStyleBackColor = true;
            // 
            // frmGenerisiIzvjestaj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 450);
            this.Controls.Add(this.flwIgraci);
            this.Controls.Add(this.btnGenerisiIzvjestaj);
            this.Controls.Add(this.txtNaziv);
            this.Controls.Add(this.lblNaziv);
            this.Controls.Add(this.lblIzvjestaj);
            this.Controls.Add(this.flwTakmicenja);
            this.Controls.Add(this.cmbIzvjestaj);
            this.Name = "frmGenerisiIzvjestaj";
            this.Text = "frmGenerisiIzvjestaj";
            this.flwIgraci.ResumeLayout(false);
            this.flwIgraci.PerformLayout();
            this.flwTakmicenja.ResumeLayout(false);
            this.flwTakmicenja.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblIzvjestaj;
        private System.Windows.Forms.ComboBox cmbIzvjestaj;
        private System.Windows.Forms.Label lblNaziv;
        private System.Windows.Forms.TextBox txtNaziv;
        private System.Windows.Forms.Button btnGenerisiIzvjestaj;
        private System.Windows.Forms.CheckBox chbJacaruka;
        private System.Windows.Forms.CheckBox chbVisina;
        private System.Windows.Forms.CheckBox chbELO;
        private System.Windows.Forms.CheckBox chbSpol;
        private System.Windows.Forms.CheckBox chbBrojPosjeta;
        private System.Windows.Forms.FlowLayoutPanel flwIgraci;
        private System.Windows.Forms.FlowLayoutPanel flwTakmicenja;
        private System.Windows.Forms.CheckBox chbBrojRundi;
        private System.Windows.Forms.CheckBox chbMinELO;
    }
}