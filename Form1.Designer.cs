namespace IletimHatti
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1486, 1055);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1255, 984);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Elektrik İletim Hattı Parametreleri";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        // ════════════════════════════════════════════════════════════════
        // Kontrol değişkenleri
        // ════════════════════════════════════════════════════════════════
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel panelLeft, panelRight;
        private System.Windows.Forms.GroupBox grpGiris, grpParametreler, grpABCD, grpKomp;
        private System.Windows.Forms.GroupBox grpKisa, grpOrta, grpUzun;
        private System.Windows.Forms.TabControl tabResults;
        private System.Windows.Forms.TabPage tabSonuclar, tabGrafikler;

        private System.Windows.Forms.Label lblGerilim, lblIletken, lblDirek;
        private System.Windows.Forms.Label lblHatUzunlugu, lblSicaklik, lblDemet, lblDevre;
        private System.Windows.Forms.Label lblGucKatsayi, lblYukDurum, lblKompLabel;

        private System.Windows.Forms.ComboBox cmbGerilim, cmbIletken, cmbDirek;
        private System.Windows.Forms.ComboBox cmbDemet, cmbDevre, cmbYukDurum, cmbKomp;
        private System.Windows.Forms.NumericUpDown nudHatUzunlugu, nudSicaklik;
        private System.Windows.Forms.TextBox txtGucKatsayi;
        private System.Windows.Forms.Button btnHesapla;

        private System.Windows.Forms.Label lblRac, lblRacVal, lblEnduktans, lblEnduktansVal;
        private System.Windows.Forms.Label lblKapasitans, lblKapasitansVal, lblEmpedans, lblEmpedansVal;
        private System.Windows.Forms.Label lblAdmitans, lblAdmitansVal;
        private System.Windows.Forms.Label lblA, lblAVal, lblB, lblBVal, lblC, lblCVal, lblD, lblDVal;
        private System.Windows.Forms.Label lblKompGR, lblKompGRVal, lblKompVer, lblKompVerVal;

        // Kısa hat
        private System.Windows.Forms.Label lblKV1k, lblV1k, lblKI1k, lblI1k, lblKgKk, lblgKk;
        private System.Windows.Forms.Label lblKaGk, lblaGk, lblKrGk, lblrGk, lblKgRk, lblgRk, lblKverk, lblverk;
        // Orta hat
        private System.Windows.Forms.Label lblKV1o, lblV1o, lblKI1o, lblI1o, lblKgKo, lblgKo;
        private System.Windows.Forms.Label lblKaGo, lblaGo, lblKrGo, lblrGo, lblKgRo, lblgRo, lblKvero, lblvero;
        // Uzun hat
        private System.Windows.Forms.Label lblKV1u, lblV1u, lblKI1u, lblI1u, lblKgKu, lblgKu;
        private System.Windows.Forms.Label lblKaGu, lblaGu, lblKrGu, lblrGu, lblKgRu, lblgRu, lblKveru, lblveru;

        private System.Windows.Forms.DataVisualization.Charting.Chart chartHat;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPV;
    }
}