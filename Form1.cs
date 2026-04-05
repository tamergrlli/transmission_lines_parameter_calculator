using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using IletimHatti.Calculators;
using IletimHatti.Helpers;
using IletimHatti.Models;

namespace IletimHatti
{
    public partial class Form1 : Form
    {
        private Iletken _secilenIletken;
        private Direk _seciliDirek;
        private double _gerilim_kV = 154;
        private double _gucKatsayisi = 0.9;
        private double _fiRad = Math.Acos(0.9);

        public Form1()
        {
            InitializeComponent();      
            DinamikArayuzuYukle();      
            YukleBaslangicVerileri();   
        }


        private void DinamikArayuzuYukle()
        {
            this.SuspendLayout();

            // 1. Nesneleri Oluşturma (Instantiations)
            this.splitMain = new SplitContainer();
            this.panelLeft = new Panel();
            this.panelRight = new Panel();
            this.grpGiris = new GroupBox();
            this.grpParametreler = new GroupBox();
            this.grpABCD = new GroupBox();
            this.grpKomp = new GroupBox();
            this.lblGerilim = new Label();
            this.cmbGerilim = new ComboBox();
            this.lblIletken = new Label();
            this.cmbIletken = new ComboBox();
            this.lblDirek = new Label();
            this.cmbDirek = new ComboBox();
            this.lblHatUzunlugu = new Label();
            this.nudHatUzunlugu = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudHatUzunlugu)).BeginInit();
            this.lblSicaklik = new Label();
            this.nudSicaklik = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudSicaklik)).BeginInit();
            this.lblDemet = new Label();
            this.cmbDemet = new ComboBox();
            this.lblDevre = new Label();
            this.cmbDevre = new ComboBox();
            this.lblGucKatsayi = new Label();
            this.txtGucKatsayi = new TextBox();
            this.lblYukDurum = new Label();
            this.cmbYukDurum = new ComboBox();
            this.lblKompLabel = new Label();
            this.cmbKomp = new ComboBox();
            this.btnHesapla = new Button();
            this.lblRac = new Label(); this.lblRacVal = new Label();
            this.lblEnduktans = new Label(); this.lblEnduktansVal = new Label();
            this.lblKapasitans = new Label(); this.lblKapasitansVal = new Label();
            this.lblEmpedans = new Label(); this.lblEmpedansVal = new Label();
            this.lblAdmitans = new Label(); this.lblAdmitansVal = new Label();
            this.lblA = new Label(); this.lblAVal = new Label();
            this.lblB = new Label(); this.lblBVal = new Label();
            this.lblC = new Label(); this.lblCVal = new Label();
            this.lblD = new Label(); this.lblDVal = new Label();
            this.lblKompGR = new Label(); this.lblKompGRVal = new Label();
            this.lblKompVer = new Label(); this.lblKompVerVal = new Label();
            this.tabResults = new TabControl();
            this.tabSonuclar = new TabPage();
            this.tabGrafikler = new TabPage();
            this.grpKisa = new GroupBox();
            this.grpOrta = new GroupBox();
            this.grpUzun = new GroupBox();

            // Etiket atamaları ve isimlendirmeler
            this.lblKV1k = new Label(); this.lblV1k = new Label(); this.lblV1k.Name = "lblV1";
            this.lblKI1k = new Label(); this.lblI1k = new Label(); this.lblI1k.Name = "lblI1";
            this.lblKgKk = new Label(); this.lblgKk = new Label(); this.lblgKk.Name = "lblgK";
            this.lblKaGk = new Label(); this.lblaGk = new Label(); this.lblaGk.Name = "lblaG";
            this.lblKrGk = new Label(); this.lblrGk = new Label(); this.lblrGk.Name = "lblrG";
            this.lblKgRk = new Label(); this.lblgRk = new Label(); this.lblgRk.Name = "lblgR";
            this.lblKverk = new Label(); this.lblverk = new Label(); this.lblverk.Name = "lblver";
            this.lblKV1o = new Label(); this.lblV1o = new Label(); this.lblV1o.Name = "lblV1";
            this.lblKI1o = new Label(); this.lblI1o = new Label(); this.lblI1o.Name = "lblI1";
            this.lblKgKo = new Label(); this.lblgKo = new Label(); this.lblgKo.Name = "lblgK";
            this.lblKaGo = new Label(); this.lblaGo = new Label(); this.lblaGo.Name = "lblaG";
            this.lblKrGo = new Label(); this.lblrGo = new Label(); this.lblrGo.Name = "lblrG";
            this.lblKgRo = new Label(); this.lblgRo = new Label(); this.lblgRo.Name = "lblgR";
            this.lblKvero = new Label(); this.lblvero = new Label(); this.lblvero.Name = "lblver";
            this.lblKV1u = new Label(); this.lblV1u = new Label(); this.lblV1u.Name = "lblV1";
            this.lblKI1u = new Label(); this.lblI1u = new Label(); this.lblI1u.Name = "lblI1";
            this.lblKgKu = new Label(); this.lblgKu = new Label(); this.lblgKu.Name = "lblgK";
            this.lblKaGu = new Label(); this.lblaGu = new Label(); this.lblaGu.Name = "lblaG";
            this.lblKrGu = new Label(); this.lblrGu = new Label(); this.lblrGu.Name = "lblrG";
            this.lblKgRu = new Label(); this.lblgRu = new Label(); this.lblgRu.Name = "lblgR";
            this.lblKveru = new Label(); this.lblveru = new Label(); this.lblveru.Name = "lblver";

            this.chartHat = new Chart();
            this.chartPV = new Chart();

            // Renk Paleti
            Color bg = Color.FromArgb(22, 25, 35);
            Color panel1 = Color.FromArgb(30, 34, 48);
            Color panel2 = Color.FromArgb(38, 43, 60);
            Color accent = Color.FromArgb(64, 190, 255);
            Color accent2 = Color.FromArgb(255, 140, 60);
            Color silver = Color.FromArgb(210, 218, 228);
            Color dim = Color.FromArgb(130, 145, 165);

            // Form Ana Ayarları
            this.BackColor = bg;
            this.ForeColor = silver;
            this.Font = new Font("Segoe UI", 9f);
            this.Width = 1000;

            // SplitContainer
            this.splitMain.Dock = DockStyle.Fill;
            this.splitMain.SplitterWidth = 5;

            this.splitMain.BackColor = bg;
            this.splitMain.Panel1.BackColor = bg;
            this.splitMain.Panel2.BackColor = bg;
            this.splitMain.Margin = new Padding(5);

            // Sol Panel
            this.panelLeft.Dock = DockStyle.Fill;
            this.panelLeft.AutoScroll = true;
            this.panelLeft.BackColor = bg;
            this.panelLeft.Padding = new Padding(5, 5, 5, 5);

            var lblBaslik = new Label
            {
                Text = "⚡  İletim Hattı Analizi",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = accent,
                Location = new Point(5, 5),
                Size = new Size(295, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.panelLeft.Controls.Add(lblBaslik);

            // GİRİŞ GRUBU
            KonfigureGrp(this.grpGiris, "  SİSTEM GİRİŞİ", panel1, silver, 5, 40, 295, 385);
            int gy = 22;
            EkleSatir(grpGiris, lblGerilim, "Gerilim Seviyesi:", dim, 5, gy); gy += 18;
            EkleCombo(grpGiris, cmbGerilim, accent, panel2, 5, gy); gy += 28;
            EkleSatir(grpGiris, lblIletken, "İletken Tipi:", dim, 5, gy); gy += 18;
            EkleCombo(grpGiris, cmbIletken, accent, panel2, 5, gy); gy += 28;
            EkleSatir(grpGiris, lblDirek, "Direk Tipi:", dim, 5, gy); gy += 18;
            EkleCombo(grpGiris, cmbDirek, accent, panel2, 5, gy); gy += 28;
            EkleSatir(grpGiris, lblHatUzunlugu, "Hat Uzunluğu (km):", dim, 5, gy); gy += 18;
            EkleNud(grpGiris, nudHatUzunlugu, 1, 1000, 200, accent, panel2, 5, gy); gy += 28;
            EkleSatir(grpGiris, lblSicaklik, "Sıcaklık (°C):", dim, 5, gy); gy += 18;
            EkleNud(grpGiris, nudSicaklik, -50, 100, 20, accent, panel2, 5, gy); gy += 28;
            EkleSatir(grpGiris, lblDemet, "Demet Sayısı:", dim, 5, gy); gy += 18;
            EkleCombo(grpGiris, cmbDemet, accent, panel2, 5, gy); gy += 28;
            EkleSatir(grpGiris, lblDevre, "Devre Sayısı:", dim, 5, gy); gy += 18;
            EkleCombo(grpGiris, cmbDevre, accent, panel2, 5, gy); gy += 28;
            EkleSatir(grpGiris, lblGucKatsayi, "Güç Katsayısı (cosφ):", dim, 5, gy); gy += 18;

            this.txtGucKatsayi.Text = "0.9";
            this.txtGucKatsayi.Location = new Point(5, gy);
            this.txtGucKatsayi.Size = new Size(278, 23);
            this.txtGucKatsayi.BackColor = panel2;
            this.txtGucKatsayi.ForeColor = accent;
            this.txtGucKatsayi.BorderStyle = BorderStyle.FixedSingle;
            this.grpGiris.Controls.Add(this.txtGucKatsayi); gy += 28;

            EkleSatir(grpGiris, lblYukDurum, "Yük Durumu:", dim, 5, gy); gy += 18;
            EkleCombo(grpGiris, cmbYukDurum, accent, panel2, 5, gy); gy += 28;
            EkleSatir(grpGiris, lblKompLabel, "Kompanzasyon:", dim, 5, gy); gy += 18;
            EkleCombo(grpGiris, cmbKomp, accent, panel2, 5, gy);
            this.grpGiris.Height = gy + 50;

            // Hesapla Butonu
            int btnY = this.grpGiris.Bottom + 5;
            this.btnHesapla.Text = "▶   H E S A P L A";
            this.btnHesapla.Location = new Point(5, btnY);
            this.btnHesapla.Size = new Size(295, 38);
            this.btnHesapla.BackColor = accent2;
            this.btnHesapla.ForeColor = Color.White;
            this.btnHesapla.FlatStyle = FlatStyle.Flat;
            this.btnHesapla.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            this.btnHesapla.FlatAppearance.BorderSize = 0;
            this.btnHesapla.Cursor = Cursors.Hand;
            this.panelLeft.Controls.Add(this.btnHesapla);

            // Parametreler Grubu
            int pY = btnY + 48;
            KonfigureGrp(this.grpParametreler, "  HAT PARAMETRELERİ", panel1, silver, 5, pY, 295, 138);
            int py2 = 22;
            EkleSonucSatiri(grpParametreler, lblRac, "Rac (Ω/km):", dim, lblRacVal, accent, 5, py2); py2 += 22;
            EkleSonucSatiri(grpParametreler, lblEnduktans, "L (mH/km):", dim, lblEnduktansVal, accent, 5, py2); py2 += 22;
            EkleSonucSatiri(grpParametreler, lblKapasitans, "C (nF/km):", dim, lblKapasitansVal, accent, 5, py2); py2 += 22;
            EkleSonucSatiri(grpParametreler, lblEmpedans, "Z (Ω):", dim, lblEmpedansVal, accent, 5, py2); py2 += 22;
            EkleSonucSatiri(grpParametreler, lblAdmitans, "Y (μS):", dim, lblAdmitansVal, accent, 5, py2);

            // ABCD Grubu
            int abY = grpParametreler.Bottom + 5;
            KonfigureGrp(this.grpABCD, "  ABCD PARAMETRELERİ", panel1, silver, 5, abY, 295, 112);
            Color cg = Color.FromArgb(100, 230, 130);
            int ay = 22;
            EkleSonucSatiri(grpABCD, lblA, "A :", dim, lblAVal, cg, 5, ay); ay += 22;
            EkleSonucSatiri(grpABCD, lblB, "B :", dim, lblBVal, cg, 5, ay); ay += 22;
            EkleSonucSatiri(grpABCD, lblC, "C :", dim, lblCVal, cg, 5, ay); ay += 22;
            EkleSonucSatiri(grpABCD, lblD, "D :", dim, lblDVal, cg, 5, ay);

            // Kompanzasyon Grubu
            int kY = grpABCD.Bottom + 5;
            KonfigureGrp(this.grpKomp, "  KOMPANZASYON SONUÇLARI", panel1, silver, 5, kY, 295, 72);
            EkleSonucSatiri(grpKomp, lblKompGR, "Gerilim Reg.:", dim, lblKompGRVal, accent2, 5, 22);
            EkleSonucSatiri(grpKomp, lblKompVer, "Verim (%):", dim, lblKompVerVal, accent2, 5, 44);

            this.panelLeft.Controls.AddRange(new Control[] { grpGiris, grpParametreler, grpABCD, grpKomp });

            // Sağ Panel ve Sekmeler
            this.tabResults.Dock = DockStyle.Fill;
            this.tabResults.BackColor = bg;
            this.tabResults.Font = new Font("Segoe UI", 9.5f);
            this.tabResults.Appearance = TabAppearance.Normal;

            this.tabSonuclar.Text = "  Hesap Sonuçları  ";
            this.tabSonuclar.BackColor = bg;
            this.tabGrafikler.Text = "  Grafikler  ";
            this.tabGrafikler.BackColor = bg;

            InşaHatSonucGrubu(grpKisa, "KISA HAT  (≤ 50 km)", lblKV1k, lblV1k, lblKI1k, lblI1k, lblKgKk, lblgKk, lblKaGk, lblaGk, lblKrGk, lblrGk, lblKgRk, lblgRk, lblKverk, lblverk, panel1, dim, silver, accent, 5, 5);
            InşaHatSonucGrubu(grpOrta, "ORTA HAT  (50 – 300 km)", lblKV1o, lblV1o, lblKI1o, lblI1o, lblKgKo, lblgKo, lblKaGo, lblaGo, lblKrGo, lblrGo, lblKgRo, lblgRo, lblKvero, lblvero, panel1, dim, silver, accent, 5, grpKisa.Bottom + 6);
            InşaHatSonucGrubu(grpUzun, "UZUN HAT  (≥ 300 km)", lblKV1u, lblV1u, lblKI1u, lblI1u, lblKgKu, lblgKu, lblKaGu, lblaGu, lblKrGu, lblrGu, lblKgRu, lblgRu, lblKveru, lblveru, panel1, dim, silver, accent, 5, grpOrta.Bottom + 6);

            this.tabSonuclar.Controls.AddRange(new Control[] { grpKisa, grpOrta, grpUzun });

            // Grafikler
            this.chartHat.Location = new Point(5, 5);
            this.chartHat.Size = new Size(900, 290);
            this.chartHat.BackColor = panel1;
            this.chartHat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            this.chartPV.Location = new Point(5, 305);
            this.chartPV.Size = new Size(900, 260);
            this.chartPV.BackColor = panel1;
            this.chartPV.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            this.tabGrafikler.Controls.Add(this.chartHat);
            this.tabGrafikler.Controls.Add(this.chartPV);

            this.tabResults.TabPages.Add(this.tabSonuclar);
            this.tabResults.TabPages.Add(this.tabGrafikler);

            // Olay (Event) Bağlantıları
            this.cmbGerilim.SelectedIndexChanged += cmbGerilim_SelectedIndexChanged;
            this.cmbIletken.SelectedIndexChanged += cmbIletken_SelectedIndexChanged;
            this.cmbDirek.SelectedIndexChanged += cmbDirek_SelectedIndexChanged;
            this.txtGucKatsayi.TextChanged += txtGucKatsayi_TextChanged;
            this.cmbYukDurum.SelectedIndexChanged += cmbYukDurum_SelectedIndexChanged;
            this.btnHesapla.Click += btnHesapla_Click;

            // Form'a Ekleme
            this.splitMain.Panel1.Controls.Add(this.panelLeft);
            this.splitMain.Panel2.Controls.Add(this.tabResults);
            this.Controls.Add(this.splitMain);

            ((System.ComponentModel.ISupportInitialize)(this.nudHatUzunlugu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSicaklik)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ════════════════════════════════════════════════════════════════
        // DİNAMİK ARAYÜZ YARDIMCI METOTLARI
        // ════════════════════════════════════════════════════════════════

        private static void KonfigureGrp(GroupBox g, string baslik, Color bg, Color fg, int x, int y, int w, int h)
        {
            g.Text = baslik; g.Location = new Point(x, y); g.Size = new Size(w, h);
            g.BackColor = bg; g.ForeColor = fg; g.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
        }

        private static void EkleSatir(Control parent, Label lbl, string text, Color fg, int x, int y)
        {
            lbl.Text = text; lbl.Location = new Point(x, y); lbl.Size = new Size(278, 16);
            lbl.ForeColor = fg; lbl.Font = new Font("Segoe UI", 8f);
            parent.Controls.Add(lbl);
        }

        private static void EkleCombo(Control parent, ComboBox cmb, Color fg, Color bg, int x, int y)
        {
            cmb.Location = new Point(x, y); cmb.Size = new Size(278, 23);
            cmb.BackColor = bg; cmb.ForeColor = fg;
            cmb.DropDownStyle = ComboBoxStyle.DropDownList; cmb.FlatStyle = FlatStyle.Flat;
            parent.Controls.Add(cmb);
        }

        private static void EkleNud(Control parent, NumericUpDown nud, decimal min, decimal max, decimal val, Color fg, Color bg, int x, int y)
        {
            nud.Minimum = min; nud.Maximum = max; nud.Value = val;
            nud.Location = new Point(x, y); nud.Size = new Size(278, 23);
            nud.BackColor = bg; nud.ForeColor = fg;
            parent.Controls.Add(nud);
        }

        private static void EkleSonucSatiri(Control parent, Label lblKey, string keyText, Color keyColor, Label lblVal, Color valColor, int x, int y)
        {
            lblKey.Text = keyText; lblKey.Location = new Point(x, y); lblKey.Size = new Size(110, 18);
            lblKey.ForeColor = keyColor; lblKey.Font = new Font("Segoe UI", 8f);
            lblVal.Text = "—"; lblVal.Location = new Point(x + 112, y); lblVal.Size = new Size(160, 18);
            lblVal.ForeColor = valColor; lblVal.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            lblVal.TextAlign = ContentAlignment.MiddleLeft;
            parent.Controls.Add(lblKey); parent.Controls.Add(lblVal);
        }

        private static void InşaHatSonucGrubu(GroupBox grp, string baslik, Label lkV1, Label lV1, Label lkI1, Label lI1, Label lkgK, Label lgK, Label lkaG, Label laG, Label lkrG, Label lrG, Label lkgR, Label lgR, Label lkver, Label lver, Color bg, Color dim, Color silver, Color accent, int x, int y)
        {
            KonfigureGrp(grp, "  " + baslik, bg, silver, x, y, 940, 120);
            grp.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            string[] keys = { "V₁:", "I₁:", "cos φ:", "P₁ (MW):", "Q₁ (MVAR):", "Ger. Reg.:", "Verim (%):" };
            Label[] keyLbls = { lkV1, lkI1, lkgK, lkaG, lkrG, lkgR, lkver };
            Label[] valLbls = { lV1, lI1, lgK, laG, lrG, lgR, lver };
            int colW = 450;
            for (int i = 0; i < 7; i++)
            {
                int col = i < 4 ? 0 : 1; int row = i < 4 ? i : i - 4;
                int cx = 8 + col * colW; int cy = 22 + row * 22;
                EkleSonucSatiri(grp, keyLbls[i], keys[i], dim, valLbls[i], accent, cx, cy);
            }
        }

        // ════════════════════════════════════════════════════════════════
        // BAŞLANGIÇ
        // ════════════════════════════════════════════════════════════════
        private void YukleBaslangicVerileri()
        {
            cmbGerilim.Items.AddRange(new[] { "154 kV", "400 kV" }); cmbGerilim.SelectedIndex = 0;
            if (Direk.TumDirekler != null) foreach (var d in Direk.TumDirekler) cmbDirek.Items.Add(d.DirekTipi);
            cmbDemet.Items.AddRange(new[] { "Tekli", "İkili", "Üçlü" }); cmbDemet.SelectedIndex = 0;
            cmbDevre.Items.AddRange(new[] { "Tek Devre", "Çift Devre" }); cmbDevre.SelectedIndex = 0;
            cmbYukDurum.Items.AddRange(new[] { "Endüktif (geç)", "Kapasitif (ileri)" }); cmbYukDurum.SelectedIndex = 0;
            cmbKomp.Items.AddRange(new[] { "%30 S2", "%30 10×S2", "%50 S2", "%50 10×S2" }); cmbKomp.SelectedIndex = 0;

            if (chartHat != null) GrafikYardimci.HazirlaGrafik(chartHat);
            if (chartPV != null) GrafikYardimci.HazirlaGrafik(chartPV);
        }

        // ════════════════════════════════════════════════════════════════
        // OLAY YÖNETİCİLERİ VE HESAPLAMA AKIŞI
        // 
        // ════════════════════════════════════════════════════════════════

        private void cmbGerilim_SelectedIndexChanged(object sender, EventArgs e)
        {
            _gerilim_kV = cmbGerilim.SelectedIndex == 0 ? 154 : 400;
            var liste = _gerilim_kV == 154 ? Iletken.Liste154kV : Iletken.Liste400kV;
            cmbIletken.Items.Clear();
            if (liste != null) cmbIletken.Items.AddRange(liste.ToArray());
            cmbIletken.SelectedIndex = -1;
            _secilenIletken = null;
        }

        private void cmbIletken_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ad = cmbIletken.SelectedItem?.ToString();
            if (Iletken.TumIletkenler != null) _secilenIletken = Iletken.TumIletkenler.FirstOrDefault(i => i.Ad == ad);
        }

        private void cmbDirek_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tip = cmbDirek.SelectedItem?.ToString();
            if (Direk.TumDirekler != null) _seciliDirek = Direk.TumDirekler.FirstOrDefault(d => d.DirekTipi == tip);
        }

        private void txtGucKatsayi_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtGucKatsayi.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double gk) && gk > 0 && gk <= 1)
            {
                _gucKatsayisi = gk;
                _fiRad = Math.Acos(gk);
                if (cmbYukDurum.SelectedIndex == 1) _fiRad = -_fiRad;
            }
        }

        private void cmbYukDurum_SelectedIndexChanged(object sender, EventArgs e)
        {
            _fiRad = cmbYukDurum.SelectedIndex == 1 ? -Math.Acos(_gucKatsayisi) : Math.Acos(_gucKatsayisi);
        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            if (!GirdiDogrula()) return;
            try { HesaplaVeGoster(); }
            catch (Exception ex) { MessageBox.Show($"Hesaplama hatası:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void HesaplaVeGoster()
        {
            double hatUzunlugu = (double)nudHatUzunlugu.Value;
            double sicaklik = (double)nudSicaklik.Value;
            int demetSayisi = cmbDemet.SelectedIndex + 1;
            bool ciftDevre = cmbDevre.SelectedIndex == 1;

            var konfig = new HatKonfigurasyonu { HatUzunlugu = hatUzunlugu, Sicaklik = sicaklik, DemetSayisi = demetSayisi, CiftDevre = ciftDevre };
            var hesap = new HatHesaplayici(_secilenIletken, _seciliDirek, konfig);

            double rac = hesap.HesaplaRac();
            double L = hesap.HesaplaEnduktans();
            double C = hesap.HesaplaKapasitans();
            hesap.HesaplaEmpedans(L, C, hatUzunlugu);
            hesap.HesaplaABCD(hatUzunlugu);

            var (P2, Q2) = hesap.HesaplaAliciGucu(L, C, _gerilim_kV, _fiRad);
            var kisaSonuc = hesap.HesaplaGondericiTaraf(Complex.One, hesap.Z, Complex.Zero, P2, _gerilim_kV, _fiRad);
            var ortaSonuc = hesap.HesaplaGondericiTaraf(hesap.A, hesap.B, hesap.C_param, P2, _gerilim_kV, _fiRad);
            var uzunSonuc = hesap.HesaplaGondericiTaraf(hesap.A, hesap.B, hesap.C_param, P2, _gerilim_kV, _fiRad);

            double I2_mag = P2 * Math.Sqrt(3) / (Math.Cos(_fiRad) * 3 * _gerilim_kV * 1e3);
            Complex I2 = Complex.FromPolarCoordinates(I2_mag, _fiRad);
            Complex I2_10x = Complex.FromPolarCoordinates(10 * I2_mag, _fiRad);
            var kompTip = (KompanzasyonTipi)cmbKomp.SelectedIndex;
            var (kompGR, kompVer) = hesap.HesaplaKompanzasyon(kompTip, P2, _gerilim_kV, _fiRad, I2, I2_10x);

            var (gP, gQ, gV) = hesap.HesaplaGrafikVerisi(P2, _gerilim_kV, _fiRad);
            var (pvP, pvV) = hesap.HesaplaPVEgrisi(L, C, _gerilim_kV, _fiRad);

            lblRacVal.Text = rac.ToString("0.0000") + " Ω/km";
            lblEnduktansVal.Text = L.ToString("0.0000") + " mH/km";
            lblKapasitansVal.Text = C.ToString("0.0000") + " nF/km";
            lblEmpedansVal.Text = $"{hesap.Z.Magnitude:F2} ∠ {hesap.Z.Phase * 180 / Math.PI:F2}°  Ω";
            lblAdmitansVal.Text = $"{hesap.Y:F4} μS";

            lblAVal.Text = $"{hesap.A.Magnitude:F4} ∠ {hesap.A.Phase * 180 / Math.PI:F3}°";
            lblBVal.Text = $"{hesap.B.Magnitude:F3} ∠ {hesap.B.Phase * 180 / Math.PI:F3}°";
            lblCVal.Text = $"{hesap.C_param.Magnitude * 1e6:F3} μ∠ {hesap.C_param.Phase * 180 / Math.PI:F2}°";
            lblDVal.Text = lblAVal.Text;

            YazSonuc(kisaSonuc, grpKisa);
            YazSonuc(ortaSonuc, grpOrta);
            YazSonuc(uzunSonuc, grpUzun);

            lblKompGRVal.Text = kompGR.ToString("0.000");
            lblKompVerVal.Text = kompVer.ToString("0.000") + " %";

            GrafikYardimci.CizHatGrafigi(chartHat, gP, gQ, gV);
            GrafikYardimci.CizPVEgrisi(chartPV, pvP, pvV);
        }

        private static void YazSonuc(GerilimAkimSonucu s, GroupBox grp)
        {
            SetLabel(grp, "lblV1", $"{s.V1_Mag:F2} ∠ {s.V1_Phase:F3}° kV");
            SetLabel(grp, "lblI1", $"{s.I1_Mag:F2} ∠ {s.I1_Phase:F3}° A");
            SetLabel(grp, "lblgK", s.GucKatsayisi.ToString("0.000"));
            SetLabel(grp, "lblaG", s.AktifGuc.ToString("0.000") + " MW");
            SetLabel(grp, "lblrG", s.ReaktifGuc.ToString("0.000") + " MVAR");
            SetLabel(grp, "lblgR", s.GerRegulasyon.ToString("0.000"));
            SetLabel(grp, "lblver", s.Verim.ToString("0.000") + " %");
        }

        private static void SetLabel(Control parent, string name, string text)
        {
            if (parent.Controls.Find(name, true).FirstOrDefault() is Label lbl) lbl.Text = text;
        }

        private bool GirdiDogrula()
        {
            if (_secilenIletken == null) { Uyar("Lütfen bir iletken seçin."); return false; }
            if (_seciliDirek == null) { Uyar("Lütfen bir direk tipi seçin."); return false; }
            if (nudHatUzunlugu.Value <= 0) { Uyar("Hat uzunluğu 0'dan büyük olmalıdır."); return false; }
            return true;
        }

        private static void Uyar(string msg) => MessageBox.Show(msg, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void Form1_Load(object sender, EventArgs e)
        {
            splitMain.Panel1MinSize = 290;
            splitMain.Panel2MinSize = 400;
            splitMain.SplitterDistance = 320;
        }
    }
}