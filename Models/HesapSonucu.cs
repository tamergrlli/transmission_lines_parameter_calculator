using System.Numerics;

namespace IletimHatti.Models
{
    public class HesapSonucu
    {
        // Hat parametreleri
        public double Rac { get; set; }
        public double Enduktans { get; set; }
        public double Kapasitans { get; set; }
        public double Empedans_Mag { get; set; }
        public double Empedans_Phase { get; set; }
        public double Admitans_Y { get; set; }

        // ABCD parametreleri
        public Complex A { get; set; }
        public Complex B { get; set; }
        public Complex C { get; set; }

        // Güç
        public double P2 { get; set; }
        public double Q2 { get; set; }

        // Gerilim/Akım sonuçları (kısa/orta/uzun hat)
        public GerilimAkimSonucu KisaHat { get; set; }
        public GerilimAkimSonucu OrtaHat { get; set; }
        public GerilimAkimSonucu UzunHat { get; set; }

        // Kompanzasyon
        public double KompGerRegulasyon { get; set; }
        public double KompVerim { get; set; }

        // Grafik verileri
        public List<double> GrafikP { get; set; } = new();
        public List<double> GrafikQ { get; set; } = new();
        public List<double> GrafikV { get; set; } = new();
        public List<double> PV_P1 { get; set; } = new();
        public List<double> PV_V1 { get; set; } = new();
    }

    public class GerilimAkimSonucu
    {
        public double V1_Mag { get; set; }
        public double V1_Phase { get; set; }
        public double I1_Mag { get; set; }
        public double I1_Phase { get; set; }
        public double GucKatsayisi { get; set; }
        public double AktifGuc { get; set; }    // MW
        public double ReaktifGuc { get; set; }  // MVAR
        public double GerRegulasyon { get; set; }
        public double Verim { get; set; }
    }
}
