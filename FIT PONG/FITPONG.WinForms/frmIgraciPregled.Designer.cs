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
            this.btnFiltriraj = new System.Windows.Forms.Button();
            this.dgvIgraci = new System.Windows.Forms.DataGridView();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
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
            // btnFiltriraj
            // 
            this.btnFiltriraj.Location = new System.Drawing.Point(623, 46);
            this.btnFiltriraj.Name = "btnFiltriraj";
            this.btnFiltriraj.Size = new System.Drawing.Size(142, 32);
            this.btnFiltriraj.TabIndex = 4;
            this.btnFiltriraj.Text = "Filtriraj";
            this.btnFiltriraj.UseVisualStyleBackColor = true;
            this.btnFiltriraj.Click += new System.EventHandler(this.btnFiltriraj_Click);
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
            this.dgvIgraci.Size = new System.Drawing.Size(727, 292);
            this.dgvIgraci.TabIndex = 5;
            this.dgvIgraci.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvIgraci_CellDoubleClick);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(525, 415);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(87, 32);
            this.btnNext.TabIndex = 6;
            this.btnNext.Text = "Naprijed";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(193, 415);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(87, 32);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "Nazad";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // frmIgraciPregled
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.dgvIgraci);
            this.Controls.Add(this.btnFiltriraj);
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
        private System.Windows.Forms.Button btnFiltriraj;
        private System.Windows.Forms.DataGridView dgvIgraci;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
    }
}