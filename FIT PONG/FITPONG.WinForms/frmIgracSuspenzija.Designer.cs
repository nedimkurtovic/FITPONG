namespace FIT_PONG.WinForms
{
    partial class frmIgracSuspenzija
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
            this.lblPrikaznoIme = new System.Windows.Forms.Label();
            this.txtPrikaznoIme = new System.Windows.Forms.TextBox();
            this.lblTipSuspenzije = new System.Windows.Forms.Label();
            this.cmbTipSuspenzije = new System.Windows.Forms.ComboBox();
            this.dtpPocetak = new System.Windows.Forms.DateTimePicker();
            this.lblPocetak = new System.Windows.Forms.Label();
            this.lblKraj = new System.Windows.Forms.Label();
            this.dtpKraj = new System.Windows.Forms.DateTimePicker();
            this.btnSuspenduj = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPrikaznoIme
            // 
            this.lblPrikaznoIme.AutoSize = true;
            this.lblPrikaznoIme.Location = new System.Drawing.Point(42, 27);
            this.lblPrikaznoIme.Name = "lblPrikaznoIme";
            this.lblPrikaznoIme.Size = new System.Drawing.Size(89, 17);
            this.lblPrikaznoIme.TabIndex = 0;
            this.lblPrikaznoIme.Text = "Prikazno ime";
            // 
            // txtPrikaznoIme
            // 
            this.txtPrikaznoIme.Location = new System.Drawing.Point(45, 47);
            this.txtPrikaznoIme.Name = "txtPrikaznoIme";
            this.txtPrikaznoIme.ReadOnly = true;
            this.txtPrikaznoIme.Size = new System.Drawing.Size(141, 22);
            this.txtPrikaznoIme.TabIndex = 1;
            // 
            // lblTipSuspenzije
            // 
            this.lblTipSuspenzije.AutoSize = true;
            this.lblTipSuspenzije.Location = new System.Drawing.Point(42, 93);
            this.lblTipSuspenzije.Name = "lblTipSuspenzije";
            this.lblTipSuspenzije.Size = new System.Drawing.Size(99, 17);
            this.lblTipSuspenzije.TabIndex = 2;
            this.lblTipSuspenzije.Text = "Tip suspenzije";
            // 
            // cmbTipSuspenzije
            // 
            this.cmbTipSuspenzije.FormattingEnabled = true;
            this.cmbTipSuspenzije.Location = new System.Drawing.Point(45, 113);
            this.cmbTipSuspenzije.Name = "cmbTipSuspenzije";
            this.cmbTipSuspenzije.Size = new System.Drawing.Size(141, 24);
            this.cmbTipSuspenzije.TabIndex = 3;
            // 
            // dtpPocetak
            // 
            this.dtpPocetak.Location = new System.Drawing.Point(45, 180);
            this.dtpPocetak.Name = "dtpPocetak";
            this.dtpPocetak.Size = new System.Drawing.Size(255, 22);
            this.dtpPocetak.TabIndex = 4;
            // 
            // lblPocetak
            // 
            this.lblPocetak.AutoSize = true;
            this.lblPocetak.Location = new System.Drawing.Point(42, 160);
            this.lblPocetak.Name = "lblPocetak";
            this.lblPocetak.Size = new System.Drawing.Size(59, 17);
            this.lblPocetak.TabIndex = 5;
            this.lblPocetak.Text = "Pocetak";
            // 
            // lblKraj
            // 
            this.lblKraj.AutoSize = true;
            this.lblKraj.Location = new System.Drawing.Point(334, 160);
            this.lblKraj.Name = "lblKraj";
            this.lblKraj.Size = new System.Drawing.Size(33, 17);
            this.lblKraj.TabIndex = 7;
            this.lblKraj.Text = "Kraj";
            // 
            // dtpKraj
            // 
            this.dtpKraj.Location = new System.Drawing.Point(337, 180);
            this.dtpKraj.Name = "dtpKraj";
            this.dtpKraj.Size = new System.Drawing.Size(255, 22);
            this.dtpKraj.TabIndex = 8;
            // 
            // btnSuspenduj
            // 
            this.btnSuspenduj.BackColor = System.Drawing.Color.Red;
            this.btnSuspenduj.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuspenduj.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSuspenduj.Location = new System.Drawing.Point(226, 292);
            this.btnSuspenduj.Name = "btnSuspenduj";
            this.btnSuspenduj.Size = new System.Drawing.Size(167, 60);
            this.btnSuspenduj.TabIndex = 12;
            this.btnSuspenduj.Text = "Suspenduj";
            this.btnSuspenduj.UseVisualStyleBackColor = false;
            this.btnSuspenduj.Click += new System.EventHandler(this.btnSuspenduj_Click);
            // 
            // frmIgracSuspenzija
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 450);
            this.Controls.Add(this.btnSuspenduj);
            this.Controls.Add(this.dtpKraj);
            this.Controls.Add(this.lblKraj);
            this.Controls.Add(this.lblPocetak);
            this.Controls.Add(this.dtpPocetak);
            this.Controls.Add(this.cmbTipSuspenzije);
            this.Controls.Add(this.lblTipSuspenzije);
            this.Controls.Add(this.txtPrikaznoIme);
            this.Controls.Add(this.lblPrikaznoIme);
            this.Name = "frmIgracSuspenzija";
            this.Text = "frmIgracSuspenzija";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrikaznoIme;
        private System.Windows.Forms.TextBox txtPrikaznoIme;
        private System.Windows.Forms.Label lblTipSuspenzije;
        private System.Windows.Forms.ComboBox cmbTipSuspenzije;
        private System.Windows.Forms.DateTimePicker dtpPocetak;
        private System.Windows.Forms.Label lblPocetak;
        private System.Windows.Forms.Label lblKraj;
        private System.Windows.Forms.DateTimePicker dtpKraj;
        private System.Windows.Forms.Button btnSuspenduj;
    }
}