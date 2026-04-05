namespace IletimHatti.Models
{
    public class Direk
    {
        public string TipNo { get; set; }
        public string DirekTipi { get; set; }

        // Tip3 (tek devre) mesafeleri
        public double D1 { get; set; }
        public double D2 { get; set; }
        public double D3 { get; set; }

        // Tip2 (çift devre) faz arası mesafeler
        public double D_ab { get; set; }
        public double D_ac { get; set; }
        public double D_bc { get; set; }
        public double D_abu { get; set; }
        public double D_bcu { get; set; }
        public double D_acu { get; set; }
        public double D_aau { get; set; }
        public double D_bbu { get; set; }
        public double D_ccu { get; set; }

        public bool IsCiftDevre => TipNo == "tip2";

        public static List<Direk> TumDirekler => new List<Direk>
        {
            new Direk { TipNo="tip2", DirekTipi="TA1", D_ab=4.32,   D_ac=8.302, D_bc=4.268, D_abu=9.19,   D_bcu=9.369,  D_acu=10.987, D_aau=7,    D_bbu=9.4,  D_ccu=7.4 },
            new Direk { TipNo="tip2", DirekTipi="N1",  D_ab=4.301,  D_ac=8.205, D_bc=4.22,  D_abu=8.9,    D_bcu=9.167,  D_acu=10.716, D_aau=6.6,  D_bbu=9.2,  D_ccu=7.2 },
            new Direk { TipNo="tip3", DirekTipi="PA",  D1=12,       D2=12,      D3=24 },
            new Direk { TipNo="tip3", DirekTipi="2B",  D1=9,        D2=18,      D3=9  },
            new Direk { TipNo="tip3", DirekTipi="3B1", D1=7.043,    D2=7.043,   D3=14.086 },
            new Direk { TipNo="tip3", DirekTipi="A2",  D1=6.8,      D2=6.8,     D3=13.6 },
            new Direk { TipNo="tip3", DirekTipi="3B",  D1=7.043,    D2=7.043,   D3=14.086 },
            new Direk { TipNo="tip3", DirekTipi="AS",  D1=6,        D2=6,       D3=12  },
            new Direk { TipNo="tip2", DirekTipi="A",   D_ab=15.091, D_ac=30,    D_bc=15.091, D_abu=21.602, D_bcu=21.602, D_acu=13.89,  D_aau=33.059, D_bbu=17.2, D_ccu=33.059 },
        };
    }
}
