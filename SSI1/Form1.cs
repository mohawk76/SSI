using System.Globalization;
using System.Text;

namespace SSI1
{
    public partial class Form1 : Form
    {
        // Stałe ziarno: każde uruchomienie daje te same wyniki, więc liczby w raporcie zgadzają się z wykresami.
        private const int Ziarno = 2005;

        // probki[s][i]: s = numer próbki (0..M-1), i = atrybut (0 = x1, 1 = x2)
        private double[][] _probki = [];

        private static readonly string KatalogWykresow =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wykresy");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dane.csv");
            _probki = WczytajCsv(path);
            txtRaport.Text = $"Wczytano {_probki.Length} próbek z {path}{Environment.NewLine}" +
                             $"Wykresy PNG zapisują się w: {KatalogWykresow}";
            RysujGrupy(new int[_probki.Length], [], "Próbki (spiralka)", null);
        }

        private static double[][] WczytajCsv(string path)
        {
            var table = new List<double[]>();

            if (!File.Exists(path))
                return [];

            foreach (var line in File.ReadLines(path))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');
                if (parts.Length < 2)
                    continue;

                if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var x) &&
                    double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
                {
                    table.Add([x, y]);
                }
            }

            return table.ToArray();
        }

        // ---------- Raporty ----------

        private void btnRaport1_Click(object sender, EventArgs e) => UruchomBezpiecznie(Raport1);
        private void btnRaport2_Click(object sender, EventArgs e) => UruchomBezpiecznie(Raport2);

        /// <summary>Raport 1: k-średnie, euklidesowa, 3 grupy, stan po 4 i po 10 iteracjach (jedno uruchomienie).</summary>
        private void Raport1()
        {
            var sb = new StringBuilder();
            KSrednie.Uruchom(_probki, m: 3, iters: 10, Odleglosci.Euklidesowa, new Random(Ziarno), stan =>
            {
                if (stan.Iteracja is 4 or 10)
                {
                    var tytul = $"Raport 1: k-średnie, euklidesowa, m=3, po {stan.Iteracja} iteracjach";
                    sb.Append(Statystyki.Formatuj(tytul, Statystyki.DlaKSrednich(_probki, stan)));
                    RysujGrupy(stan.Przynaleznosc, stan.Srodki, tytul, $"raport1_iter{stan.Iteracja}.png");
                }
            });
            txtRaport.Text = sb.ToString();
        }

        /// <summary>
        /// Raport 2: k-średnie, odległość |x1A - x1B|, 4 grupy, 10 iteracji.
        /// PDF prosi o wykres "po 4 iteracjach" przy 10 iteracjach (pewnie literówka), więc zapisujemy oba.
        /// </summary>
        private void Raport2()
        {
            var sb = new StringBuilder();
            KSrednie.Uruchom(_probki, m: 4, iters: 10, Odleglosci.TylkoX1, new Random(Ziarno), stan =>
            {
                if (stan.Iteracja is 4 or 10)
                {
                    var tytul = $"Raport 2: k-średnie, tylko x1, m=4, po {stan.Iteracja} iteracjach";
                    sb.Append(Statystyki.Formatuj(tytul, Statystyki.DlaKSrednich(_probki, stan)));
                    RysujGrupy(stan.Przynaleznosc, stan.Srodki, tytul, $"raport2_iter{stan.Iteracja}.png");
                }
            });
            txtRaport.Text = sb.ToString();
        }

        private void UruchomBezpiecznie(Action raport)
        {
            if (_probki.Length == 0)
            {
                MessageBox.Show("Brak danych: nie wczytano próbek z dane.csv.", "Brak danych", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                raport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas generowania raportu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- Wykres ----------

        /// <summary>
        /// Rysuje każdą grupę jako osobną serię i środki jako gwiazdki.
        /// Jeśli podano plikPng, zapisuje wykres do katalogu "wykresy" obok exe.
        /// </summary>
        private void RysujGrupy(int[] przynaleznosc, double[][] srodki, string tytul, string? plikPng)
        {
            var plot = formsPlot1.Plot;
            plot.Clear();
            var paleta = new ScottPlot.Palettes.Category10();

            int liczbaGrup = Math.Max(srodki.Length, przynaleznosc.Length == 0 ? 0 : przynaleznosc.Max() + 1);
            for (int j = 0; j < liczbaGrup; j++)
            {
                var xs = new List<double>();
                var ys = new List<double>();
                for (int s = 0; s < _probki.Length; s++)
                {
                    if (przynaleznosc[s] != j)
                        continue;
                    xs.Add(_probki[s][0]);
                    ys.Add(_probki[s][1]);
                }
                if (xs.Count == 0)
                    continue;

                var seria = plot.Add.Scatter(xs.ToArray(), ys.ToArray());
                seria.LineStyle = ScottPlot.LineStyle.None;
                seria.MarkerShape = ScottPlot.MarkerShape.OpenSquare;
                seria.MarkerSize = 7;
                seria.Color = paleta.GetColor(j);
                seria.LegendText = srodki.Length == 0 ? "Próbki" : $"Grupa {j + 1}";
            }

            if (srodki.Length > 0)
            {
                var sx = srodki.Select(v => v[0]).ToArray();
                var sy = srodki.Select(v => v[1]).ToArray();
                var srodkiSeria = plot.Add.Scatter(sx, sy);
                srodkiSeria.LineStyle = ScottPlot.LineStyle.None;
                srodkiSeria.MarkerShape = ScottPlot.MarkerShape.Asterisk;
                srodkiSeria.MarkerSize = 16;
                srodkiSeria.MarkerLineWidth = 2;
                srodkiSeria.Color = ScottPlot.Colors.Red;
                srodkiSeria.LegendText = "Środki grup";
            }

            plot.Title(tytul);
            plot.XLabel("x1");
            plot.YLabel("x2");
            plot.ShowLegend();
            plot.Axes.SquareUnits();
            formsPlot1.Refresh();

            if (plikPng is not null)
            {
                Directory.CreateDirectory(KatalogWykresow);
                plot.SavePng(Path.Combine(KatalogWykresow, plikPng), 1000, 800);
            }
        }
    }
}
