using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Permutations_of_N
{
    internal class Permutations_of_N
    {
        private static int okPermutations = 0;

        private static void Main(string[] args)
        {
            List<int> arr = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
            permute(arr, 0, 4);
            Console.WriteLine("OK Permutations: {0}", okPermutations);

            Console.ReadKey();
        }

        private static void permute(List<int> arry, int i, int T)
        {
            if (i == arry.Count - 1) // reached a permutation
            {
                check(arry);
            }

            if (T == 1)
            {
                for (int j = i; j <= arry.Count - 1; j++)
                {
                    swap(arry, i, j);
                    permute(arry, i + 1, T);
                    swap(arry, i, j); //backtrack
                }
            }
            else
            {
                Parallel.Invoke(
                    () =>
                    {
                        for (int j = i; j <= arry.Count - 1; j += 2)
                        {
                            swap(arry, i, j);
                            permute(arry, i + 1, T / 2);
                            swap(arry, i, j); //backtrack
                        }
                    },
                    () =>
                    {
                        for (int j = i + 1; j <= arry.Count - 1; j += 2)
                        {
                            swap(arry, i, j);
                            permute(arry, i + 1, T - T / 2);
                            swap(arry, i, j); //backtrack
                        }
                    }
                    );
            }
        }

        private static void swap(List<int> arr, int startpos, int endpos)
        {
            int tmp = arr[startpos];
            arr[startpos] = arr[endpos];
            arr[endpos] = tmp;
        }

        private static void check(List<int> arr)
        {
            if (arr[0] > 0)
            {
                Interlocked.Increment(ref okPermutations);
            }
        }
    }
}