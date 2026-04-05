using System.Numerics;
using IletimHatti.Models;

namespace IletimHatti.Calculators
{

    public class HatHesaplayici
    {
        private readonly Iletken _iletken;
        private readonly Direk _direk;
        private readonly HatKonfigurasyonu _konfig;

        public double Rac     { get; private set; }
        public double R       { get; private set; }   // Toplam direnç (Ω)
        public double XL      { get; private set; }   // Toplam reaktans (Ω)
        public double Y       { get; private set; }   // Toplam admitans (μS)
        public Complex Z      { get; private set; }
        public Complex A      { get; private set; }
        public Complex B      { get; private set; }
        public Complex C_param{ get; private set; }

        public HatHesaplayici(Iletken iletken, Direk direk, HatKonfigurasyonu konfig)
        {
            _iletken = iletken ?? throw new ArgumentNullException(nameof(iletken));
            _direk   = direk   ?? throw new ArgumentNullException(nameof(direk));
            _konfig  = konfig  ?? throw new ArgumentNullException(nameof(konfig));
        }

        // ════════════════════════════════════════════════════════════════
        // 1. TEMEL PARAMETRELER
        // ════════════════════════════════════════════════════════════════

        >
        public double HesaplaRac()
        {
            const double T = 228.0;
            double direnc = _iletken.Direnc;

            
            if (_konfig.Sicaklik > 0)
                direnc = direnc * ((T + _konfig.Sicaklik) / (T + 20.0));

            double Xr = Math.Sqrt((4 * 2 * Math.PI * 50) / (direnc * 10000));

            if (Xr < 3)
                Rac = direnc * ((Math.Sqrt(1 + Math.Pow(Xr, 4.0) / 48) + 1) / 2);
            else if (Xr > 3)
                Rac = direnc * ((Xr / (2 * Math.Sqrt(2))) + 0.26);
            else
                Rac = direnc;

            return Rac;
        }

       
        public double HesaplaGMD()
        {
            double GMD;
            if (!_konfig.CiftDevre)
            {
                double D_ab = _direk.D1, D_ac = _direk.D2, D_bc = _direk.D3;
                GMD = Math.Pow(D_ab * D_ac * D_bc, 1.0 / 3.0);
            }
            else
            {
                double D_AB = Math.Pow(_direk.D_ab * _direk.D_ab * _direk.D_abu * _direk.D_abu, 1.0 / 4.0);
                double D_AC = Math.Pow(_direk.D_ac * _direk.D_ac * _direk.D_acu * _direk.D_acu, 1.0 / 4.0);
                double D_BC = Math.Pow(_direk.D_bc * _direk.D_bc * _direk.D_bcu * _direk.D_bcu, 1.0 / 4.0);
                GMD = Math.Pow(D_AB * D_AC * D_BC, 1.0 / 3.0);
            }
            return GMD;
        }

        
        public double HesaplaGMR()
        {
            double GMR;
            double r_m = _iletken.Anma_Capi * 0.001 / 2.0; // mm → m yarıçap

            if (_iletken.TelSayisi == 26)
                GMR = 0.809 * r_m;
            else if (_iletken.TelSayisi == 54)
                GMR = 0.810 * r_m;
            else if (_iletken.TelSayisi == 45)
                GMR = 0.826 * r_m;
            else
                GMR = 0.07 * r_m;

            if (!_konfig.CiftDevre)
            {
                if (_konfig.DemetSayisi == 2)
                    GMR = Math.Sqrt(GMR * _iletken.DemetMesafe);
                else if (_konfig.DemetSayisi == 3)
                    GMR = Math.Pow(GMR * _iletken.DemetMesafe * _iletken.DemetMesafe, 1.0 / 3.0);
            }
            else
            {
                if (_konfig.DemetSayisi == 2)
                {
                    double GMR_A = Math.Pow(GMR * _direk.D_aau, 1.0 / 2.0);
                    double GMR_B = Math.Pow(GMR * _direk.D_bbu, 1.0 / 2.0);
                    double GMR_C = Math.Pow(GMR * _direk.D_ccu, 1.0 / 2.0);
                    GMR = Math.Pow(GMR_A * GMR_B * GMR_C, 1.0 / 3.0);
                }
                else if (_konfig.DemetSayisi == 3)
                {
                    double GMR_d = Math.Pow(GMR * _iletken.DemetMesafe * _iletken.DemetMesafe, 1.0 / 3.0);
                    double GMR_A = Math.Pow(GMR_d * _direk.D_aau, 1.0 / 2.0);
                    double GMR_B = Math.Pow(GMR_d * _direk.D_bbu, 1.0 / 2.0);
                    double GMR_C = Math.Pow(GMR_d * _direk.D_ccu, 1.0 / 2.0);
                    GMR = Math.Pow(GMR_A * GMR_B * GMR_C, 1.0 / 3.0);
                }
                else // çift devre tekli demet
                {
                    double r_A = Math.Sqrt(_iletken.Anma_Capi * 0.001 * _direk.D_aau / 2);
                    double r_B = Math.Sqrt(_iletken.Anma_Capi * 0.001 * _direk.D_bbu / 2);
                    double r_C = Math.Sqrt(_iletken.Anma_Capi * 0.001 * _direk.D_ccu / 2);
                    GMR = Math.Pow(r_A * r_B * r_C, 1.0 / 3.0);
                }
            }
            return GMR;
        }

        /// <summary>Endüktans (mH/km). L = 0.2 * ln(GMD/GMR)</summary>
        public double HesaplaEnduktans()
        {
            return 0.2 * Math.Log(HesaplaGMD() / HesaplaGMR());
        }

        /// <summary>Kapasitans (nF/km). 
        public double HesaplaKapasitans()
        {
            double GMD = HesaplaGMD();
            double c   = 0;

            if (!_konfig.CiftDevre)
            {
                if (_konfig.DemetSayisi == 1)
                {
                    c = 0.0556 / Math.Log(GMD * 1000 / (_iletken.Anma_Capi / 2));
                }
                else if (_konfig.DemetSayisi == 2)
                {
                    double r_d = Math.Sqrt(_iletken.Anma_Capi * 0.001 * _iletken.DemetMesafe / 2);
                    c = 0.0556 / Math.Log(GMD / r_d);
                }
                else if (_konfig.DemetSayisi == 3)
                {
                    double r_3d = Math.Pow(_iletken.Anma_Capi * 0.001 * _iletken.DemetMesafe * _iletken.DemetMesafe / 2, 1.0 / 3.0);
                    c = 0.0556 / Math.Log(GMD / r_3d);
                }
            }
            else
            {
                if (_konfig.DemetSayisi == 1)
                {
                    double r_A = Math.Sqrt(_iletken.Anma_Capi * 0.001 * _direk.D_aau / 2);
                    double r_B = Math.Sqrt(_iletken.Anma_Capi * 0.001 * _direk.D_bbu / 2);
                    double r_C = Math.Sqrt(_iletken.Anma_Capi * 0.001 * _direk.D_ccu / 2);
                    double r_d = Math.Pow(r_A * r_B * r_C, 1.0 / 3.0);
                    c = 0.0556 / Math.Log(GMD / r_d);
                }
                else if (_konfig.DemetSayisi == 3)
                {
                    
                    double r_A = Math.Sqrt(_iletken.Anma_Capi * 0.001 * _direk.D_aau / 2);
                    double r_B = Math.Sqrt(_iletken.Anma_Capi * 0.001 * _direk.D_bbu / 2);
                    double r_C = Math.Sqrt(_iletken.Anma_Capi * 0.001 * _direk.D_ccu / 2);
                    double r_d = Math.Pow(r_A * r_B * r_C, 1.0 / 3.0);
                    c = 0.0556 / Math.Log(GMD / r_d);
                }
            }

            return c * 1000; // → nF/km
        }

        // ════════════════════════════════════════════════════════════════
        // 2. EMPEDANS & ABCD
        // ════════════════════════════════════════════════════════════════

        public void HesaplaEmpedans(double L_mHkm, double C_nFkm, double hatUzunlugu)
        {
            double omega = 2 * Math.PI * 50;
            double XL_km = omega * L_mHkm * 0.001;
            double C_Fkm = C_nFkm * 1e-9;

            R  = Rac * hatUzunlugu;
            XL = XL_km * hatUzunlugu;
            Y  = omega * C_Fkm * 1000000 * hatUzunlugu; // μS 
            Z  = new Complex(R, XL);
        }

        public void HesaplaABCD(double hatUzunlugu)
        {
            double Y_S = Y * 0.000001; // μS → S

            if (hatUzunlugu >= 0 && hatUzunlugu <= 50)
            {
                A       = Complex.One;
                B       = Z;
                C_param = Complex.Zero;
            }
            else if (hatUzunlugu > 50 && hatUzunlugu < 300)
            {
                Complex z1 = new Complex(R, XL);
                Complex y1 = new Complex(0.0, Y_S);
                A       = 1.0 + (y1 * z1) / 2.0;
                B       = z1;
                C_param = y1 * (1.0 + (z1 * y1) / 4.0);
            }
            else
            {
                double R_km  = R  / hatUzunlugu;
                double XL_km = XL / hatUzunlugu;
                double Y_km  = Y_S / hatUzunlugu;

                Complex z1 = new Complex(R_km, XL_km);
                Complex y1 = new Complex(0.0, Y_km);

                double gama_l_mag = hatUzunlugu * Math.Sqrt(z1.Magnitude * Y_km);
                double gama_l_ph  = (z1.Phase + y1.Phase) / 2.0;
                double zc_mag     = Math.Sqrt(z1.Magnitude / Y_km);
                double zc_ph      = (z1.Phase - y1.Phase) / 2.0;

                Complex GAMA_L = new Complex(gama_l_mag * Math.Cos(gama_l_ph),
                                             gama_l_mag * Math.Sin(gama_l_ph));
                Complex Zc     = new Complex(zc_mag * Math.Cos(zc_ph),
                                             zc_mag * Math.Sin(zc_ph));

                A       = Complex.Cosh(GAMA_L);
                B       = Zc * Complex.Sinh(GAMA_L);
                C_param = Complex.Sinh(GAMA_L) / Zc;
            }
        }

        // ════════════════════════════════════════════════════════════════
        // 3. GÜÇ HESABI
        // ════════════════════════════════════════════════════════════════

        public (double P2_W, double Q2_VAR) HesaplaAliciGucu(
            double L_mHkm, double C_nFkm, double gerilim_kV, double fi_rad)
        {
            double L_Hkm = L_mHkm * 1e-3;
            double C_Fkm = C_nFkm * 1e-9;
            double s = (gerilim_kV * gerilim_kV) / Math.Sqrt((L_Hkm * 1e6) / C_Fkm);
            double P2 = s * Math.Cos(fi_rad) * 1e6;
            double Q2 = s * Math.Sin(fi_rad) * 1e6;
            return (P2, Q2);
        }

        // ════════════════════════════════════════════════════════════════
        // 4. GÖNDERME TARAFI
        // ════════════════════════════════════════════════════════════════


        public GerilimAkimSonucu HesaplaGondericiTaraf(
            Complex A_p, Complex B_p, Complex C_p,
            double P2_W, double gerilim_kV, double fi_rad)
        {
            double I2_mag = (P2_W * Math.Sqrt(3)) / (Math.Cos(fi_rad) * 3 * gerilim_kV * 1e3);
            Complex I2    = Complex.FromPolarCoordinates(I2_mag, fi_rad);

            Complex V1 = (A_p * gerilim_kV * 1e3 / Math.Sqrt(3)) + I2 * B_p;
            Complex I1 = (C_p  * gerilim_kV * 1e3 / Math.Sqrt(3)) + A_p * I2;

            double HbGk = Math.Abs(V1.Phase - I1.Phase);
            double P1   = 3.0 * V1.Magnitude * I1.Magnitude * Math.Cos(HbGk);
            double Q1   = 3.0 * V1.Magnitude * I1.Magnitude * Math.Sin(HbGk);

            
            double G_reg = (((V1.Magnitude / A_p.Magnitude) - (gerilim_kV * 1e3))
                           / (gerilim_kV * 1e3)) * 10;
            double Verim = (P2_W / P1) * 100;

            return new GerilimAkimSonucu
            {
                V1_Mag        = V1.Magnitude / 1e3,
                V1_Phase      = V1.Phase * 180 / Math.PI,
                I1_Mag        = I1.Magnitude,
                I1_Phase      = I1.Phase * 180 / Math.PI,
                GucKatsayisi  = Math.Cos(HbGk),
                AktifGuc      = P1 / 1e6,
                ReaktifGuc    = Q1 / 1e6,
                GerRegulasyon = G_reg,
                Verim         = Verim
            };
        }

        // ════════════════════════════════════════════════════════════════
        // 5. GRAFİK VERİLERİ
        // ════════════════════════════════════════════════════════════════

        public (List<double> P_MW, List<double> Q_MVAR, List<double> V_kV)
            HesaplaGrafikVerisi(double P2_W, double gerilim_kV, double fi_rad)
        {
            var pList = new List<double>();
            var qList = new List<double>();
            var vList = new List<double>();

            double R_km  = R  / _konfig.HatUzunlugu;
            double XL_km = XL / _konfig.HatUzunlugu;
            double Y_km  = (Y * 1e-6) / _konfig.HatUzunlugu;

            for (int km = 20; km <= 200; km += 20)
            {
                var (A_g, B_g, C_g) = UzunHatABCD(R_km, XL_km, Y_km, km);

                double I2_mag = (P2_W * Math.Sqrt(3)) / (Math.Cos(fi_rad) * 3 * gerilim_kV * 1e3);
                Complex I2g   = Complex.FromPolarCoordinates(I2_mag, fi_rad);

                Complex hbg = (A_g * gerilim_kV * 1e3 / Math.Sqrt(3)) + I2g * B_g;
                Complex hba = C_g * gerilim_kV * 1e3 / Math.Sqrt(3) + A_g * I2g;

                // Orijinal: işaret korumalı (Math.Abs yok)
                double HbGk = hbg.Phase - hba.Phase;

                pList.Add(3.0 * hbg.Magnitude * hba.Magnitude * Math.Cos(HbGk) / 1e6);
                qList.Add(3.0 * hbg.Magnitude * hba.Magnitude * Math.Sin(HbGk) / 1e6);
                vList.Add(hbg.Magnitude / 1e3);
            }
            return (pList, qList, vList);
        }

        public (List<double> P1_MW, List<double> V1_kV) HesaplaPVEgrisi(
            double L_mHkm, double C_nFkm, double gerilim_kV, double fi_rad)
        {
            double L_Hkm  = L_mHkm * 1e-3;
            double C_Fkm  = C_nFkm * 1e-9;
            double s_base = (gerilim_kV * gerilim_kV) / Math.Sqrt((L_Hkm * 1e6) / C_Fkm);

            double R_km  = R  / _konfig.HatUzunlugu;
            double XL_km = XL / _konfig.HatUzunlugu;
            double Y_km  = (Y * 1e-6) / _konfig.HatUzunlugu;

            var (A_g, B_g, C_g) = UzunHatABCD(R_km, XL_km, Y_km, _konfig.HatUzunlugu);

            var p1List = new List<double>();
            var v1List = new List<double>();

            for (double i = 0; i <= 10; i++)
            {
                double k    = 0.5 + i * 0.1;
                double P2_W = s_base * k * Math.Cos(fi_rad) * 1e6;

                // Orijinal: Math.Cos(-fi_rad)
                double I2_mag = (P2_W * Math.Sqrt(3)) / (Math.Cos(-fi_rad) * 3 * gerilim_kV * 1e3);
                Complex I2    = Complex.FromPolarCoordinates(I2_mag, fi_rad);

                Complex V1c = (A_g * gerilim_kV * 1e3 / Math.Sqrt(3)) + I2 * B_g;
                Complex I1c = C_g * gerilim_kV * 1e3 / Math.Sqrt(3) + A_g * I2;
                double delta = Math.Abs(V1c.Phase - I1c.Phase);

                p1List.Add(3.0 * V1c.Magnitude * I1c.Magnitude * Math.Cos(delta) / 1e6);
                v1List.Add(V1c.Magnitude / 1e3);
            }
            return (p1List, v1List);
        }

        // ════════════════════════════════════════════════════════════════
        // 6. KOMPANZASYON
        // ════════════════════════════════════════════════════════════════

        public (double GerReg, double Verim) HesaplaKompanzasyon(
            KompanzasyonTipi tip, double P2_W, double gerilim_kV, double fi_rad,
            Complex I2_normal, Complex I2_10x)
        {
            double oran   = (tip == KompanzasyonTipi.Yuzde30_S2 || tip == KompanzasyonTipi.Yuzde30_10S2) ? 0.3 : 0.5;
            bool   use10x = (tip == KompanzasyonTipi.Yuzde30_10S2 || tip == KompanzasyonTipi.Yuzde50_10S2);

            Complex seriCap  = new Complex(0, -Z.Imaginary * oran);
            Complex B_cap    = A * seriCap + 1 * B;
            Complex I2_used  = use10x ? I2_10x : I2_normal;

            Complex komp_V1  = (A * gerilim_kV * 1e3 / Math.Sqrt(3)) + (B_cap * I2_used);
            Complex komp_I1  = (C_param * gerilim_kV * 1e3 / Math.Sqrt(3)) + (A * I2_used);

            double komp_ger_reg = 100 * ((komp_V1.Magnitude / A.Magnitude) - (gerilim_kV * 1e3 / Math.Sqrt(3)))
                                       / (gerilim_kV * 1e3 / Math.Sqrt(3));
            double komp_P1  = 3 * komp_V1.Magnitude * komp_I1.Magnitude * Math.Cos(fi_rad);
            double komp_ver = (use10x ? 1000.0 : 100.0) * P2_W / komp_P1;

            return (komp_ger_reg, komp_ver);
        }

        // ════════════════════════════════════════════════════════════════
        // YARDIMCI
        // ════════════════════════════════════════════════════════════════

        private static (Complex A, Complex B, Complex C) UzunHatABCD(
            double R_km, double XL_km, double Y_km, double km)
        {
            Complex z1 = new Complex(R_km, XL_km);
            Complex y1 = new Complex(0.0, Y_km);

            double gama_mag = km * Math.Sqrt(z1.Magnitude * Y_km);
            double gama_ph  = (z1.Phase + y1.Phase) / 2.0;
            double zc_mag   = Math.Sqrt(z1.Magnitude / Y_km);
            double zc_ph    = (z1.Phase - y1.Phase) / 2.0;

            Complex GAMA_L = new Complex(gama_mag * Math.Cos(gama_ph), gama_mag * Math.Sin(gama_ph));
            Complex Zc     = new Complex(zc_mag   * Math.Cos(zc_ph),   zc_mag   * Math.Sin(zc_ph));

            return (Complex.Cosh(GAMA_L), Zc * Complex.Sinh(GAMA_L), Complex.Sinh(GAMA_L) / Zc);
        }
    }

    public class HatKonfigurasyonu
    {
        public double HatUzunlugu { get; set; }
        public double Sicaklik    { get; set; }
        public int    DemetSayisi { get; set; } = 1;
        public bool   CiftDevre   { get; set; } = false;
    }

    public enum KompanzasyonTipi
    {
        Yuzde30_S2,
        Yuzde30_10S2,
        Yuzde50_S2,
        Yuzde50_10S2
    }
}
