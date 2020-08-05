namespace FIT_PONG.WinForms
{
    partial class frmIzvjestajIgraci
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
            this.rpvIgraci = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // rpvIgraci
            // 
            this.rpvIgraci.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rpvIgraci.Location = new System.Drawing.Point(0, 0);
            this.rpvIgraci.Name = "rpvIgraci";
            this.rpvIgraci.ServerReport.BearerToken = null;
            this.rpvIgraci.Size = new System.Drawing.Size(800, 450);
            this.rpvIgraci.TabIndex = 0;
            // 
            // frmIzvjestajIgraci
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rpvIgraci);
            this.Name = "frmIzvjestajIgraci";
            this.Text = "frmIzvjestajIgraci";
            this.Load += new System.EventHandler(this.frmIzvjestajIgraci_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rpvIgraci;
    }
}