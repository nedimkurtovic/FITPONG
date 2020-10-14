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
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chartStatistika = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblBrojLogiranihKorisnika = new System.Windows.Forms.Label();
            this.txtLogiraniKorisnici = new System.Windows.Forms.TextBox();
            this.txtMaxLogiranihKorisnika = new System.Windows.Forms.TextBox();
            this.lblMaksimalnoLogiranihKorisnika = new System.Windows.Forms.Label();
            this.txtZagusenjeAplikacije = new System.Windows.Forms.TextBox();
            this.lblZagusenjeApliakcije = new System.Windows.Forms.Label();
            this.btnNazad = new System.Windows.Forms.Button();
            this.btnNaprijed = new System.Windows.Forms.Button();
            this.btnOsvjeziStanje = new System.Windows.Forms.Button();
            this.btnDobaviAktivnosti = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartStatistika)).BeginInit();
            this.SuspendLayout();
            // 
            // chartStatistika
            // 
            chartArea1.Name = "ChartArea1";
            this.chartStatistika.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartStatistika.Legends.Add(legend1);
            this.chartStatistika.Location = new System.Drawing.Point(41, 183);
            this.chartStatistika.Margin = new System.Windows.Forms.Padding(2);
            this.chartStatistika.Name = "chartStatistika";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Broj korisnika";
            this.chartStatistika.Series.Add(series1);
            this.chartStatistika.Size = new System.Drawing.Size(526, 163);
            this.chartStatistika.TabIndex = 0;
            this.chartStatistika.Text = "Statistika";
            title1.Name = "Title1";
            title1.Text = "Broj korisnika po danu";
            this.chartStatistika.Titles.Add(title1);
            // 
            // lblBrojLogiranihKorisnika
            // 
            this.lblBrojLogiranihKorisnika.AutoSize = true;
            this.lblBrojLogiranihKorisnika.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrojLogiranihKorisnika.Location = new System.Drawing.Point(37, 15);
            this.lblBrojLogiranihKorisnika.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBrojLogiranihKorisnika.Name = "lblBrojLogiranihKorisnika";
            this.lblBrojLogiranihKorisnika.Size = new System.Drawing.Size(166, 20);
            this.lblBrojLogiranihKorisnika.TabIndex = 1;
            this.lblBrojLogiranihKorisnika.Text = "Broj logiranih korisnika";
            // 
            // txtLogiraniKorisnici
            // 
            this.txtLogiraniKorisnici.Location = new System.Drawing.Point(301, 17);
            this.txtLogiraniKorisnici.Margin = new System.Windows.Forms.Padding(2);
            this.txtLogiraniKorisnici.Name = "txtLogiraniKorisnici";
            this.txtLogiraniKorisnici.Size = new System.Drawing.Size(266, 20);
            this.txtLogiraniKorisnici.TabIndex = 2;
            // 
            // txtMaxLogiranihKorisnika
            // 
            this.txtMaxLogiranihKorisnika.Location = new System.Drawing.Point(301, 57);
            this.txtMaxLogiranihKorisnika.Margin = new System.Windows.Forms.Padding(2);
            this.txtMaxLogiranihKorisnika.Name = "txtMaxLogiranihKorisnika";
            this.txtMaxLogiranihKorisnika.Size = new System.Drawing.Size(266, 20);
            this.txtMaxLogiranihKorisnika.TabIndex = 4;
            // 
            // lblMaksimalnoLogiranihKorisnika
            // 
            this.lblMaksimalnoLogiranihKorisnika.AutoSize = true;
            this.lblMaksimalnoLogiranihKorisnika.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaksimalnoLogiranihKorisnika.Location = new System.Drawing.Point(37, 55);
            this.lblMaksimalnoLogiranihKorisnika.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMaksimalnoLogiranihKorisnika.Name = "lblMaksimalnoLogiranihKorisnika";
            this.lblMaksimalnoLogiranihKorisnika.Size = new System.Drawing.Size(222, 20);
            this.lblMaksimalnoLogiranihKorisnika.TabIndex = 3;
            this.lblMaksimalnoLogiranihKorisnika.Text = "Maksimalno logiranih korisnika";
            // 
            // txtZagusenjeAplikacije
            // 
            this.txtZagusenjeAplikacije.Location = new System.Drawing.Point(301, 96);
            this.txtZagusenjeAplikacije.Margin = new System.Windows.Forms.Padding(2);
            this.txtZagusenjeAplikacije.Name = "txtZagusenjeAplikacije";
            this.txtZagusenjeAplikacije.Size = new System.Drawing.Size(266, 20);
            this.txtZagusenjeAplikacije.TabIndex = 6;
            // 
            // lblZagusenjeApliakcije
            // 
            this.lblZagusenjeApliakcije.AutoSize = true;
            this.lblZagusenjeApliakcije.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZagusenjeApliakcije.Location = new System.Drawing.Point(37, 96);
            this.lblZagusenjeApliakcije.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblZagusenjeApliakcije.Name = "lblZagusenjeApliakcije";
            this.lblZagusenjeApliakcije.Size = new System.Drawing.Size(202, 20);
            this.lblZagusenjeApliakcije.TabIndex = 5;
            this.lblZagusenjeApliakcije.Text = "Datum zagušenja aplikacije";
            // 
            // btnNazad
            // 
            this.btnNazad.Location = new System.Drawing.Point(65, 364);
            this.btnNazad.Name = "btnNazad";
            this.btnNazad.Size = new System.Drawing.Size(75, 23);
            this.btnNazad.TabIndex = 7;
            this.btnNazad.Text = "Nazad";
            this.btnNazad.UseVisualStyleBackColor = true;
            this.btnNazad.Click += new System.EventHandler(this.btnNazad_Click);
            // 
            // btnNaprijed
            // 
            this.btnNaprijed.Location = new System.Drawing.Point(475, 364);
            this.btnNaprijed.Name = "btnNaprijed";
            this.btnNaprijed.Size = new System.Drawing.Size(75, 23);
            this.btnNaprijed.TabIndex = 8;
            this.btnNaprijed.Text = "Naprijed";
            this.btnNaprijed.UseMnemonic = false;
            this.btnNaprijed.UseVisualStyleBackColor = true;
            this.btnNaprijed.Click += new System.EventHandler(this.btnNaprijed_Click);
            // 
            // btnOsvjeziStanje
            // 
            this.btnOsvjeziStanje.Location = new System.Drawing.Point(396, 137);
            this.btnOsvjeziStanje.Name = "btnOsvjeziStanje";
            this.btnOsvjeziStanje.Size = new System.Drawing.Size(75, 23);
            this.btnOsvjeziStanje.TabIndex = 9;
            this.btnOsvjeziStanje.Text = "Osvjezi";
            this.btnOsvjeziStanje.UseVisualStyleBackColor = true;
            this.btnOsvjeziStanje.Click += new System.EventHandler(this.btnOsvjeziStanje_Click);
            // 
            // btnDobaviAktivnosti
            // 
            this.btnDobaviAktivnosti.Location = new System.Drawing.Point(271, 364);
            this.btnDobaviAktivnosti.Name = "btnDobaviAktivnosti";
            this.btnDobaviAktivnosti.Size = new System.Drawing.Size(75, 23);
            this.btnDobaviAktivnosti.TabIndex = 10;
            this.btnDobaviAktivnosti.Text = "Dobavi";
            this.btnDobaviAktivnosti.UseVisualStyleBackColor = true;
            this.btnDobaviAktivnosti.Click += new System.EventHandler(this.btnDobaviAktivnosti_Click);
            // 
            // frmStatistikaStranice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 401);
            this.Controls.Add(this.btnDobaviAktivnosti);
            this.Controls.Add(this.btnOsvjeziStanje);
            this.Controls.Add(this.btnNaprijed);
            this.Controls.Add(this.btnNazad);
            this.Controls.Add(this.txtZagusenjeAplikacije);
            this.Controls.Add(this.lblZagusenjeApliakcije);
            this.Controls.Add(this.txtMaxLogiranihKorisnika);
            this.Controls.Add(this.lblMaksimalnoLogiranihKorisnika);
            this.Controls.Add(this.txtLogiraniKorisnici);
            this.Controls.Add(this.lblBrojLogiranihKorisnika);
            this.Controls.Add(this.chartStatistika);
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.Button btnNazad;
        private System.Windows.Forms.Button btnNaprijed;
        private System.Windows.Forms.Button btnOsvjeziStanje;
        private System.Windows.Forms.Button btnDobaviAktivnosti;
    }
}