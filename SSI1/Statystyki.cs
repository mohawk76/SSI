using System.Globalization;
using System.Text;

namespace SSI1
{
    /// <summary>Dane jednej grupy do raportu. Dla pustej grupy Min/Max = double.NaN.</summary>
    public record StatystykaGrupy(
        int Grupa,
        double[] Srodek,
        int Liczba,
        double MinX1, double MaxX1,
        double MinX2, double MaxX2);

    public static class Statystyki
    {
        /// <summary>
        /// Raporty 1 i 2: dla każdej grupy j środek V[j], liczba próbek z u[s] == j
        /// oraz min/max x1 (probki[s][0]) i x2 (probki[s][1]) tych próbek.
        /// </summary>
        public static List<StatystykaGrupy> DlaKSrednich(double[][] probki, StanKSrednich stan)
        {
            var grupy = new List<StatystykaGrupy>();
            int m = stan.Srodki.Length;
            for (int j = 0; j < m; j++)
            {
                var srodek = stan.Srodki[j];
                var probkiGrupy = probki.Where((_, s) => stan.Przynaleznosc[s] == j).ToArray();
                int liczba = probkiGrupy.Length;
                double minX1 = liczba > 0 ? probkiGrupy.Min(p => p[0]) : double.NaN;
                double maxX1 = liczba > 0 ? probkiGrupy.Max(p => p[0]) : double.NaN;
                double minX2 = liczba > 0 ? probkiGrupy.Min(p => p[1]) : double.NaN;
                double maxX2 = liczba > 0 ? probkiGrupy.Max(p => p[1]) : double.NaN;
                grupy.Add(new StatystykaGrupy(j, srodek, liczba, minX1, maxX1, minX2, maxX2));
            }

            return grupy;
        }

        /// <summary>Tabelka tekstowa do wklejenia w raport (gotowe).</summary>
        public static string Formatuj(string tytul, IEnumerable<StatystykaGrupy> grupy)
        {
            static string F(double v) => double.IsNaN(v) ? "-" : v.ToString("0.0000", CultureInfo.InvariantCulture);

            var sb = new StringBuilder();
            sb.AppendLine(tytul);
            sb.AppendLine("Grupa | Środek (x1; x2)     | Liczba | x1 min..max        | x2 min..max");
            foreach (var g in grupy)
            {
                var srodek = string.Join("; ", g.Srodek.Select(F));
                sb.AppendLine($"{g.Grupa + 1,5} | ({srodek,-17}) | {g.Liczba,6} | {F(g.MinX1)}..{F(g.MaxX1),-8} | {F(g.MinX2)}..{F(g.MaxX2)}");
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
