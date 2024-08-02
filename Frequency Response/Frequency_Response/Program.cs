// Виведення української мови та символів
Console.OutputEncoding = System.Text.Encoding.UTF8;

// Вхідні дані та розрахунок Ω
double fg = 30, fa = 150, Ω = (2 * Math.PI) * (fg/fa);
int N = 3;

Console.WriteLine("Ω = {0:0.000}", Ω);

// a буде у вигляді матриці, щоб у майбутньому можна було змінювати N
double[] a = new double[N + 1];

for (int i = 0; i < a.Length; i++)
{
    if (i == 0)
    {
        a[i] = Ω/Math.PI;
    }
    else
    {
        a[i] = Math.Sin(i * Ω)/(Math.PI * i);
    }
    Console.WriteLine("a[{0}] = {1:0.00}", i, a[i]);
}

Console.WriteLine();
Console.WriteLine("y_n = {0:0.00} * x_n + {1:0.00} * (x_n-1 + x_n+1) + {2:0.00} * (x_n-2 + x_n+2) + {3:0.00} * (x_n-3 + x_n+3)", a[0], a[1], a[2], a[3]);
Console.WriteLine("|H(jw)| = |{0:0.00} + {1:0.00} * cos(ωT_a) + {2:0.00} * cos(2ωT_a) + {3:0.00} * cos(3ωT_a)|", a[0], 2 * a[1], 2 * a[2], 2 * a[3]);

Console.WriteLine();
Console.WriteLine("y_n_злаг = {0:0.00} * x_n + {1:0.000} * (x_n-1 + x_n+1) + {2:0.0000} * (x_n-2 + x_n+2)", a[0], a[1] * 0.75, a[2] * 0.25);
Console.WriteLine("|H_злаг(jw)| = |{0:0.00} + {1:0.00} * cos(ωT_a) + {2:0.0000} * cos(2ωT_a)|", a[0], 2 * a[1] * 0.75, 2 * a[2] * 0.25);