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
            this.btnGenerisiIzvjestaj.Location = new System.Drawing.Point(214, 123);
            this.btnGenerisiIzvjestaj.Name = "btnGenerisiIzvjestaj";
            this.btnGenerisiIzvjestaj.Size = new System.Drawing.Size(177, 40);
            this.btnGenerisiIzvjestaj.TabIndex = 6;
            this.btnGenerisiIzvjestaj.Text = "Generisi izvjestaj";
            this.btnGenerisiIzvjestaj.UseVisualStyleBackColor = true;
            this.btnGenerisiIzvjestaj.Click += new System.EventHandler(this.btnGenerisiIzvjestaj_Click);
            // 
            // frmGenerisiIzvjestaj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 450);
            this.Controls.Add(this.btnGenerisiIzvjestaj);
            this.Controls.Add(this.txtNaziv);
            this.Controls.Add(this.lblNaziv);
            this.Controls.Add(this.lblIzvjestaj);
            this.Controls.Add(this.cmbIzvjestaj);
            this.Name = "frmGenerisiIzvjestaj";
            this.Text = "frmGenerisiIzvjestaj";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblIzvjestaj;
        private System.Windows.Forms.ComboBox cmbIzvjestaj;
        private System.Windows.Forms.Label lblNaziv;
        private System.Windows.Forms.TextBox txtNaziv;
        private System.Windows.Forms.Button btnGenerisiIzvjestaj;
    }
}