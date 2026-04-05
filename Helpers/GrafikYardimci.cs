using System.Windows.Forms.DataVisualization.Charting;

namespace IletimHatti.Helpers
{
    public static class GrafikYardimci
    {
        public static void HazirlaGrafik(Chart chart)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            var area = new ChartArea("MainArea");
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(60, 60, 60);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(60, 60, 60);
            area.BackColor = Color.FromArgb(28, 32, 40);
            area.AxisX.LabelStyle.ForeColor = Color.Silver;
            area.AxisY.LabelStyle.ForeColor = Color.Silver;
            area.AxisX.TitleForeColor = Color.Silver;
            area.AxisY.TitleForeColor = Color.Silver;
            area.AxisX.LineColor = Color.DimGray;
            area.AxisY.LineColor = Color.DimGray;
            chart.ChartAreas.Add(area);
        }

        public static void CizHatGrafigi(Chart chart,
            List<double> pList, List<double> qList, List<double> vList)
        {
            HazirlaGrafik(chart);

            int[] xKm = { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200 };
            Color[] renkler = {
                Color.FromArgb(255, 99, 99),
                Color.FromArgb(99, 200, 255),
                Color.FromArgb(120, 255, 150)
            };
            string[] etiketler = { "P (MW)", "Q (MVAR)", "V (kV)" };
            List<double>[] veriler = { pList, qList, vList };

            for (int i = 0; i < 3; i++)
            {
                var s = new Series(etiketler[i])
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    Color = renkler[i],
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 6,
                    IsValueShownAsLabel = true,
                    LabelFormat = "0.00",
                    LabelForeColor = renkler[i]
                };
                for (int j = 0; j < Math.Min(veriler[i].Count, xKm.Length); j++)
                    s.Points.AddXY(xKm[j], veriler[i][j]);
                chart.Series.Add(s);
            }

            chart.ChartAreas[0].AxisX.Title = "Mesafe (km)";
            chart.ChartAreas[0].AxisY.Title = "Değer";
            chart.Legends.Clear();
            var legend = new Legend { BackColor = Color.FromArgb(28, 32, 40), ForeColor = Color.Silver };
            chart.Legends.Add(legend);
        }

        public static void CizPVEgrisi(Chart chart,
            List<double> p1List, List<double> v1List)
        {
            HazirlaGrafik(chart);

            var s = new Series("P-V")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(255, 165, 50),
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                IsValueShownAsLabel = false
            };

            int n = Math.Min(p1List.Count, v1List.Count);
            for (int j = 0; j < n; j++)
                s.Points.AddXY(p1List[j], v1List[j]);

            chart.Series.Add(s);
            var area = chart.ChartAreas[0];
            area.AxisX.Title = "P (MW)";
            area.AxisY.Title = "V (kV)";

            if (p1List.Count > 0)
            {
                area.AxisX.Minimum = Math.Floor(p1List.Min());
                area.AxisX.Maximum = Math.Ceiling(p1List.Max());
                area.AxisY.Minimum = Math.Floor(v1List.Min());
                area.AxisY.Maximum = Math.Ceiling(v1List.Max());
            }
        }
    }
}
