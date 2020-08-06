namespace FIT_PONG.WinForms
{
    partial class frmStatistikaStranice
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartStatistika = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblBrojLogiranihKorisnika = new System.Windows.Forms.Label();
            this.txtLogiraniKorisnici = new System.Windows.Forms.TextBox();
            this.txtMaxLogiranihKorisnika = new System.Windows.Forms.TextBox();
            this.lblMaksimalnoLogiranihKorisnika = new System.Windows.Forms.Label();
            this.txtZagusenjeAplikacije = new System.Windows.Forms.TextBox();
            this.lblZagusenjeApliakcije = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartStatistika)).BeginInit();
            this.SuspendLayout();
            // 
            // chartStatistika
            // 
            chartArea1.Name = "ChartArea1";
            this.chartStatistika.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartStatistika.Legends.Add(legend1);
            this.chartStatistika.Location = new System.Drawing.Point(41, 21);
            this.chartStatistika.Name = "chartStatistika";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartStatistika.Series.Add(series1);
            this.chartStatistika.Size = new System.Drawing.Size(690, 210);
            this.chartStatistika.TabIndex = 0;
            this.chartStatistika.Text = "Statistika";
            // 
            // lblBrojLogiranihKorisnika
            // 
            this.lblBrojLogiranihKorisnika.AutoSize = true;
            this.lblBrojLogiranihKorisnika.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrojLogiranihKorisnika.Location = new System.Drawing.Point(38, 269);
            this.lblBrojLogiranihKorisnika.Name = "lblBrojLogiranihKorisnika";
            this.lblBrojLogiranihKorisnika.Size = new System.Drawing.Size(206, 25);
            this.lblBrojLogiranihKorisnika.TabIndex = 1;
            this.lblBrojLogiranihKorisnika.Text = "Broj logiranih korisnika";
            // 
            // txtLogiraniKorisnici
            // 
            this.txtLogiraniKorisnici.Location = new System.Drawing.Point(321, 269);
            this.txtLogiraniKorisnici.Name = "txtLogiraniKorisnici";
            this.txtLogiraniKorisnici.Size = new System.Drawing.Size(146, 22);
            this.txtLogiraniKorisnici.TabIndex = 2;
            // 
            // txtMaxLogiranihKorisnika
            // 
            this.txtMaxLogiranihKorisnika.Location = new System.Drawing.Point(321, 311);
            this.txtMaxLogiranihKorisnika.Name = "txtMaxLogiranihKorisnika";
            this.txtMaxLogiranihKorisnika.Size = new System.Drawing.Size(146, 22);
            this.txtMaxLogiranihKorisnika.TabIndex = 4;
            // 
            // lblMaksimalnoLogiranihKorisnika
            // 
            this.lblMaksimalnoLogiranihKorisnika.AutoSize = true;
            this.lblMaksimalnoLogiranihKorisnika.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaksimalnoLogiranihKorisnika.Location = new System.Drawing.Point(38, 307);
            this.lblMaksimalnoLogiranihKorisnika.Name = "lblMaksimalnoLogiranihKorisnika";
            this.lblMaksimalnoLogiranihKorisnika.Size = new System.Drawing.Size(277, 25);
            this.lblMaksimalnoLogiranihKorisnika.TabIndex = 3;
            this.lblMaksimalnoLogiranihKorisnika.Text = "Maksimalno logiranih korisnika";
            // 
            // txtZagusenjeAplikacije
            // 
            this.txtZagusenjeAplikacije.Location = new System.Drawing.Point(321, 348);
            this.txtZagusenjeAplikacije.Name = "txtZagusenjeAplikacije";
            this.txtZagusenjeAplikacije.Size = new System.Drawing.Size(146, 22);
            this.txtZagusenjeAplikacije.TabIndex = 6;
            // 
            // lblZagusenjeApliakcije
            // 
            this.lblZagusenjeApliakcije.AutoSize = true;
            this.lblZagusenjeApliakcije.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZagusenjeApliakcije.Location = new System.Drawing.Point(38, 348);
            this.lblZagusenjeApliakcije.Name = "lblZagusenjeApliakcije";
            this.lblZagusenjeApliakcije.Size = new System.Drawing.Size(189, 25);
            this.lblZagusenjeApliakcije.TabIndex = 5;
            this.lblZagusenjeApliakcije.Text = "Zagusenje aplikacije";
            // 
            // frmStatistikaStranice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtZagusenjeAplikacije);
            this.Controls.Add(this.lblZagusenjeApliakcije);
            this.Controls.Add(this.txtMaxLogiranihKorisnika);
            this.Controls.Add(this.lblMaksimalnoLogiranihKorisnika);
            this.Controls.Add(this.txtLogiraniKorisnici);
            this.Controls.Add(this.lblBrojLogiranihKorisnika);
            this.Controls.Add(this.chartStatistika);
            this.Name = "frmStatistikaStranice";
            this.Text = "frmStatistikaStranice";
            ((System.ComponentModel.ISupportInitialize)(this.chartStatistika)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartStatistika;
        private System.Windows.Forms.Label lblBrojLogiranihKorisnika;
        private System.Windows.Forms.TextBox txtLogiraniKorisnici;
        private System.Windows.Forms.TextBox txtMaxLogiranihKorisnika;
        private System.Windows.Forms.Label lblMaksimalnoLogiranihKorisnika;
        private System.Windows.Forms.TextBox txtZagusenjeAplikacije;
        private System.Windows.Forms.Label lblZagusenjeApliakcije;
    }
}