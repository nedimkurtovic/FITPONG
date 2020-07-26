namespace FIT_PONG.WinForms
{
    partial class frmIgraciPregled
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
            this.txtELO = new System.Windows.Forms.TextBox();
            this.lblELO = new System.Windows.Forms.Label();
            this.btnFiltriraj = new System.Windows.Forms.Button();
            this.dgvIgraci = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIgraci)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPrikaznoIme
            // 
            this.lblPrikaznoIme.AutoSize = true;
            this.lblPrikaznoIme.Location = new System.Drawing.Point(35, 36);
            this.lblPrikaznoIme.Name = "lblPrikaznoIme";
            this.lblPrikaznoIme.Size = new System.Drawing.Size(89, 17);
            this.lblPrikaznoIme.TabIndex = 0;
            this.lblPrikaznoIme.Text = "Prikazno ime";
            // 
            // txtPrikaznoIme
            // 
            this.txtPrikaznoIme.Location = new System.Drawing.Point(38, 56);
            this.txtPrikaznoIme.Name = "txtPrikaznoIme";
            this.txtPrikaznoIme.Size = new System.Drawing.Size(179, 22);
            this.txtPrikaznoIme.TabIndex = 1;
            // 
            // txtELO
            // 
            this.txtELO.Location = new System.Drawing.Point(255, 56);
            this.txtELO.Name = "txtELO";
            this.txtELO.Size = new System.Drawing.Size(157, 22);
            this.txtELO.TabIndex = 3;
            // 
            // lblELO
            // 
            this.lblELO.AutoSize = true;
            this.lblELO.Location = new System.Drawing.Point(252, 36);
            this.lblELO.Name = "lblELO";
            this.lblELO.Size = new System.Drawing.Size(36, 17);
            this.lblELO.TabIndex = 2;
            this.lblELO.Text = "ELO";
            // 
            // btnFiltriraj
            // 
            this.btnFiltriraj.Location = new System.Drawing.Point(474, 55);
            this.btnFiltriraj.Name = "btnFiltriraj";
            this.btnFiltriraj.Size = new System.Drawing.Size(142, 23);
            this.btnFiltriraj.TabIndex = 4;
            this.btnFiltriraj.Text = "Filtriraj";
            this.btnFiltriraj.UseVisualStyleBackColor = true;
            // 
            // dgvIgraci
            // 
            this.dgvIgraci.AllowUserToAddRows = false;
            this.dgvIgraci.AllowUserToDeleteRows = false;
            this.dgvIgraci.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIgraci.Location = new System.Drawing.Point(38, 110);
            this.dgvIgraci.Name = "dgvIgraci";
            this.dgvIgraci.ReadOnly = true;
            this.dgvIgraci.RowHeadersWidth = 51;
            this.dgvIgraci.RowTemplate.Height = 24;
            this.dgvIgraci.Size = new System.Drawing.Size(727, 311);
            this.dgvIgraci.TabIndex = 5;
            // 
            // frmIgraciPregled
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvIgraci);
            this.Controls.Add(this.btnFiltriraj);
            this.Controls.Add(this.txtELO);
            this.Controls.Add(this.lblELO);
            this.Controls.Add(this.txtPrikaznoIme);
            this.Controls.Add(this.lblPrikaznoIme);
            this.Name = "frmIgraciPregled";
            this.Text = "frmIgraciPregled";
            ((System.ComponentModel.ISupportInitialize)(this.dgvIgraci)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrikaznoIme;
        private System.Windows.Forms.TextBox txtPrikaznoIme;
        private System.Windows.Forms.TextBox txtELO;
        private System.Windows.Forms.Label lblELO;
        private System.Windows.Forms.Button btnFiltriraj;
        private System.Windows.Forms.DataGridView dgvIgraci;
    }
}