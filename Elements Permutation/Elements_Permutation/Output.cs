using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements_Permutation
{
    static class Output
    {
        // Виводить матрицю
        static public void OutputArray(int[,] array)
        {
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                for (int x = 0; x <= array.GetUpperBound(1); x++)
                {
                    Console.Write("――――--");
                }
                Console.WriteLine();
                for (int j = 0; j <= array.GetUpperBound(1); j++)
                {
                    if (array[i, j] == 0)
                    {
                        Console.Write("".ToString().PadLeft(3) + "|".ToString().PadLeft(3));
                    }
                    else
                    {
                        Console.Write(array[i, j].ToString().PadLeft(3) + "|".ToString().PadLeft(3));
                    }
                }
                Console.WriteLine();
            }
            for (int x = 0; x <= array.GetUpperBound(1); x++)
            {
                Console.Write("――――-―");
            }
            Console.WriteLine();
        }
    }
}