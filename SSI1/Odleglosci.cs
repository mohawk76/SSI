namespace SSI1
{
    /// <summary>
    /// Miary odległości między dwoma punktami n-wymiarowymi (a.Length == b.Length == n).
    /// Każda metoda pasuje do typu Func&lt;double[], double[], double&gt;, więc można ją
    /// przekazać do algorytmu jako parametr, np. KSrednie.Uruchom(..., Odleglosci.Euklidesowa, ...).
    /// </summary>
    public static class Odleglosci
    {
        // sqrt( suma (a_i - b_i)^2 )
        public static double Euklidesowa(double[] a, double[] b)
        {
            return Math.Sqrt(a.Zip(b, (ai, bi) => (ai - bi) * (ai - bi)).Sum());
        }

        // suma |a_i - b_i|
        public static double Manhattan(double[] a, double[] b)
        {
            return a.Zip(b, (ai, bi) => Math.Abs(ai - bi)).Sum();
        }

        // max |a_i - b_i|
        public static double Czebyszewa(double[] a, double[] b)
        {
            return a.Zip(b, (ai, bi) => Math.Abs(ai - bi)).Max();
        }

        // ( suma |a_i - b_i|^mink )^(1/mink), mink > 0
        // Użycie jako parametr: (a, b) => Odleglosci.Minkowskiego(a, b, 3)
        public static double Minkowskiego(double[] a, double[] b, double mink)
        {
            return Math.Pow(a.Zip(b, (ai, bi) => Math.Abs(ai - bi)).Sum(x => Math.Pow(x, mink)), 1.0 / mink);
        }

        // Raport 2: |a_1 - b_1|, czyli tylko x1 (indeks 0), x2 ignorowane
        public static double TylkoX1(double[] a, double[] b)
        {
            return Math.Abs(a[0] - b[0]);
        }
    }
}
