# Elektrik İletim Hattı Hesaplayıcı

## Proje Yapısı

```
IletimHatti/
│
├── Models/
│   ├── Iletken.cs          ← İletken özellikleri + statik veri listesi
│   ├── Direk.cs            ← Direk özellikleri + statik veri listesi
│   └── HesapSonucu.cs      ← Sonuç veri modelleri
│
├── Calculators/
│   └── HatHesaplayici.cs   ← TÜM hesaplama mantığı (saf matematik, UI yok)
│       - HesaplaRac()
│       - HesaplaGMD() / HesaplaGMR()
│       - HesaplaEnduktans() / HesaplaKapasitans()
│       - HesaplaEmpedans()
│       - HesaplaABCD()         (kısa/orta/uzun hat otomatik seçimi)
│       - HesaplaAliciGucu()
│       - HesaplaGondericiTaraf()
│       - HesaplaGrafikVerisi()
│       - HesaplaPVEgrisi()
│       - HesaplaKompanzasyon()
│
├── Helpers/
│   └── GrafikYardimci.cs   ← Chart çizim yardımcıları (sadece görsel)
│
├── Form1.cs                ← Arayüz + olay yöneticileri + HesaplaVeGoster()
├── Form1.Designer.cs       ← Minimal stub (BuildUI() Form1.cs'de)
├── Program.cs
└── IletimHatti.csproj
```

## Sınıf Sorumlulukları

| Sınıf | Sorumluluk |
|---|---|
| `Iletken` | Veri modeli + 13 iletken tanımı |
| `Direk` | Veri modeli + 9 direk tanımı |
| `HatKonfigurasyonu` | Hesap parametreleri (uzunluk, sıcaklık, demet, devre) |
| `HatHesaplayici` | Tüm elektriksel hesaplamalar |
| `GrafikYardimci` | Chart stillemesi ve veri çizimi |
| `Form1` | UI kurulumu, olaylar, sonuç gösterimi |

## Hat Tipi Seçimi

Hesaplayıcı hat uzunluğuna göre otomatik model seçer:
- **≤ 50 km** → Kısa hat (A=D=1, B=Z, C=0)  
- **50–300 km** → Orta hat (nominal π modeli)  
- **≥ 300 km** → Uzun hat (dağıtık parametre, hiperbolik)

## Derleme

```
dotnet build
dotnet run
```

## Gereksinimler
- .NET 8.0 Windows
- System.Windows.Forms (WinForms)
- System.Windows.Forms.DataVisualization (Chart kontrolü)
# transmission_lines_parameter_calculator
# transmission_lines_parameter_calculator
