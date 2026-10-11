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
            

        }

        private static DataTable WczytajCsv(string path)
        {
            var table = new DataTable();
            table.Columns.Add("X", typeof(double));
            table.Columns.Add("Y", typeof(double));

            if (!File.Exists(path))
                return table;

            foreach (var line in File.ReadLines(path))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');
                if (parts.Length < 2)
                    continue;

                if (double.TryParse(parts[0], out var x) &&
                    double.TryParse(parts[1], out var y))
                {
                    table.Rows.Add(x, y);
                }
            }

            return table;
        }
    }
}
