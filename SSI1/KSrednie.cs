namespace SSI1
{
    /// <summary>Stan algorytmu po danej iteracji (kopie tablic, bezpieczne do zapamiętania).</summary>
    /// <param name="Srodki">V[j][i]: i-ty atrybut środka j-tej grupy.</param>
    /// <param name="Przynaleznosc">u[s]: indeks grupy (0..m-1), do której należy próbka s.</param>
    public record StanKSrednich(int Iteracja, double[][] Srodki, int[] Przynaleznosc);

    public static class KSrednie
    {
        /// <param name="probki">probki[s][i]: i-ty atrybut s-tej próbki (M x n).</param>
        /// <param name="m">Liczba grup.</param>
        /// <param name="iters">Liczba iteracji.</param>
        /// <param name="odleglosc">Miara odległości, np. Odleglosci.Euklidesowa.</param>
        /// <param name="rng">Generator losowy (stałe ziarno = powtarzalne wyniki raportu).</param>
        /// <param name="poIteracji">Wywoływane po każdej iteracji; raport wybiera z niego stan po 4. i 10. iteracji.</param>
        public static StanKSrednich Uruchom(
            double[][] probki,
            int m,
            int iters,
            Func<double[], double[], double> odleglosc,
            Random rng,
            Action<StanKSrednich>? poIteracji = null)
        {
            int M = probki.Length;
            int n = probki[0].Length;

            var V = Macierze.Utworz(m, n);
            var u = new int[M];

            // 1. Wybierz losowo m RÓŻNYCH próbek i uznaj je za środki grup (V).
            //    Tasujemy indeksy wszystkich próbek i bierzemy pierwsze m, więc próbki się nie powtarzają.
            //    Wartości kopiujemy (Clone), a nie referencję do wiersza próbki,
            //    bo w kroku 2.2 nadpisujemy środek i zmieniłoby to przy okazji samą próbkę.
            var indeksy = Enumerable.Range(0, M).ToArray();
            rng.Shuffle(indeksy);
            for (int j = 0; j < m; j++)
                V[j] = (double[])probki[indeksy[j]].Clone();


            for (int iter = 1; iter <= iters; iter++)
            {
                // 2.1. Dla każdej próbki s:
                //   2.1.1. policz odległość od każdego środka V[j],
                //   2.1.2. u[s] = indeks najbliższego środka.
                //   Wybieramy środek o najmniejszej dotąd znalezionej odległości (ostra nierówność:
                //   przy remisie zostaje środek o niższym indeksie).
                for (int s = 0; s < M; s++)
                {
                    double najmniejsza = double.MaxValue;
                    for (int j = 0; j < m; j++)
                    {
                        double d = odleglosc(probki[s], V[j]);
                        if (d < najmniejsza)
                        {
                            najmniejsza = d;
                            u[s] = j;
                        }
                    }
                }


                // 2.2. Dla każdej grupy j:
                //   2.2.1. Xgr = próbki s, dla których u[s] == j,
                //   2.2.2. jeśli Xgr jest puste, pomiń grupę (środek zostaje bez zmian),
                //   2.2.3. V[j][i] = średnia i-tego atrybutu próbek z Xgr (dla każdego i = 0..n-1).
                //   Średnia jest z OBU atrybutów także przy mierze TylkoX1; miara wpływa tylko na krok 2.1.
                for (int j = 0; j < m; j++)
                {
                    var Xgr = new List<double[]>();
                    for (int s = 0; s < M; s++)
                    {
                        if (u[s] == j)
                            Xgr.Add(probki[s]);
                    }
                    if (Xgr.Count > 0)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            V[j][i] = Xgr.Average(x => x[i]);
                        }
                    }
                }

                poIteracji?.Invoke(new StanKSrednich(iter, Macierze.Kopiuj(V), (int[])u.Clone()));
            }

            return new StanKSrednich(iters, V, u);
        }
    }
}
