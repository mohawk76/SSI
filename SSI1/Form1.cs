using ScottPlot.Plottables;
using System.Data;
using System.Globalization;

namespace SSI1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dane.csv");
            var dataTable = WczytajCsv(path);
            RysujWykres(dataTable);
        }

        private static List<(double X, double Y)> WczytajCsv(string path)
        {
            var table = new List<(double X, double Y)>();

            if (!File.Exists(path))
                return table;

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
                    table.Add((x, y));
                }
            }

            return table;
        }

        private void RysujWykres(List<(double X, double Y)> dataTable)
        {
            var xValues = new double[dataTable.Count];
            var yValues = new double[dataTable.Count];
            for (int i = 0; i < dataTable.Count; i++)
            {
                xValues[i] = dataTable[i].X;
                yValues[i] = dataTable[i].Y;
            }
            var scatter = formsPlot1.Plot.Add.Scatter(xValues, yValues);
            scatter.MarkerSize = 5;
            scatter.LineStyle = ScottPlot.LineStyle.None;
            scatter.Color = ScottPlot.Color.FromColor(Color.Blue);
            formsPlot1.Refresh();
        }
    }
}
