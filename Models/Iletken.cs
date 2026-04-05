namespace IletimHatti.Models
{
    public class Iletken
    {
        public string Ad { get; set; }
        public double KesitAlani { get; set; }
        public double Direnc { get; set; }
        public double Anma_Capi { get; set; }
        public double Kapasitans { get; set; }
        public double TelSayisi { get; set; }
        public double DemetMesafe { get; set; }

        public static List<Iletken> TumIletkenler => new List<Iletken>
        {
            new Iletken { Ad = "SWALLOW",   KesitAlani = 26.69,  Direnc = 1.0742, Anma_Capi = 7.14,  Kapasitans = 1,     TelSayisi = 6  },
            new Iletken { Ad = "SPARROW",   KesitAlani = 33.59,  Direnc = 0.8543, Anma_Capi = 8.01,  Kapasitans = 0.015, TelSayisi = 6  },
            new Iletken { Ad = "ROBIONE",   KesitAlani = 44.7,   Direnc = 0.6410, Anma_Capi = 9.24,  Kapasitans = 0.020, TelSayisi = 6  },
            new Iletken { Ad = "RAVEN",     KesitAlani = 53,     Direnc = 0.5362, Anma_Capi = 10.11, Kapasitans = 0.020, TelSayisi = 6  },
            new Iletken { Ad = "PIGEON",    KesitAlani = 85,     Direnc = 0.3366, Anma_Capi = 12.75, Kapasitans = 1,     TelSayisi = 6  },
            new Iletken { Ad = "PARTRIDGE", KesitAlani = 134.87, Direnc = 0.2140, Anma_Capi = 16.28, Kapasitans = 0.015, TelSayisi = 26 },
            new Iletken { Ad = "OSTRICH",   KesitAlani = 152.19, Direnc = 0.1897, Anma_Capi = 17.28, Kapasitans = 0.020, TelSayisi = 26 },
            new Iletken { Ad = "HAWK",      KesitAlani = 241.65, Direnc = 0.1194, Anma_Capi = 21.77, Kapasitans = 0.020, TelSayisi = 26, DemetMesafe = 0.4 },
            new Iletken { Ad = "DRAKE",     KesitAlani = 402.56, Direnc = 0.0715, Anma_Capi = 28.11, Kapasitans = 1,     TelSayisi = 26, DemetMesafe = 0.4 },
            new Iletken { Ad = "CONDOR",    KesitAlani = 402.33, Direnc = 0.0718, Anma_Capi = 27.72, Kapasitans = 0.015, TelSayisi = 54 },
            new Iletken { Ad = "RAIL",      KesitAlani = 483.4,  Direnc = 0.0599, Anma_Capi = 29.60, Kapasitans = 0.020, TelSayisi = 45, DemetMesafe = 0.4 },
            new Iletken { Ad = "CARDINAL",  KesitAlani = 484.53, Direnc = 0.0597, Anma_Capi = 30.42, Kapasitans = 0.020, TelSayisi = 54, DemetMesafe = 0.4 },
            new Iletken { Ad = "PHEASANT",  KesitAlani = 645.08, Direnc = 0.0499, Anma_Capi = 35.1,  Kapasitans = 0.020, TelSayisi = 54, DemetMesafe = 0.4 },
        };

        public static List<string> Liste154kV => new List<string>
            { "HAWK", "DRAKE", "CONDOR", "CARDINAL", "RAIL", "PHEASANT" };

        public static List<string> Liste400kV => new List<string>
            { "RAIL", "CARDINAL", "PHEASANT" };
    }
}
