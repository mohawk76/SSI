namespace SSI1
{
    /// <summary>Drobne helpery do tablic (gotowe, nie trzeba ich zmieniać).</summary>
    public static class Macierze
    {
        /// <summary>Tablica wiersze x kolumny wypełniona zerami.</summary>
        public static double[][] Utworz(int wiersze, int kolumny)
        {
            var t = new double[wiersze][];
            for (int i = 0; i < wiersze; i++)
                t[i] = new double[kolumny];
            return t;
        }

        /// <summary>Głęboka kopia, żeby zapisany stan nie zmieniał się w kolejnych iteracjach.</summary>
        public static double[][] Kopiuj(double[][] zrodlo)
        {
            var t = new double[zrodlo.Length][];
            for (int i = 0; i < zrodlo.Length; i++)
                t[i] = (double[])zrodlo[i].Clone();
            return t;
        }
    }
}
