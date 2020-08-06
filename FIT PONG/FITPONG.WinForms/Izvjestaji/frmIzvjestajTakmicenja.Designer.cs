namespace FIT_PONG.WinForms.Izvjestaji
{
    partial class frmIzvjestajTakmicenja
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
            this.rpvTakmicenja = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // rpvTakmicenja
            // 
            this.rpvTakmicenja.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rpvTakmicenja.Location = new System.Drawing.Point(0, 0);
            this.rpvTakmicenja.Name = "rpvTakmicenja";
            this.rpvTakmicenja.ServerReport.BearerToken = null;
            this.rpvTakmicenja.Size = new System.Drawing.Size(800, 450);
            this.rpvTakmicenja.TabIndex = 0;
            // 
            // frmIzvjestajTakmicenja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rpvTakmicenja);
            this.Name = "frmIzvjestajTakmicenja";
            this.Text = "frmIzvjestajTakmicenja";
            this.Load += new System.EventHandler(this.frmIzvjestajTakmicenja_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rpvTakmicenja;
    }
}