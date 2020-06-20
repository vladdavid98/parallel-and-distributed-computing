using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Parallel_Quicksort
{
    internal class Program
    {
        private static List<int> Unite(List<int> a, int pivot, List<int> b)
        {
            List<int> result = a;
            result.Add(pivot);
            result.AddRange(b);

            return result;
        }

        private static List<int> QuickSort(List<int> v, int T)
        {
            if (v.Count() <= 1)
            {
                return v;
            }

            List<int> left = new List<int>();
            List<int> right = new List<int>();

            int pivot = v[v.Count / 2];
            v.RemoveAt(v.Count / 2);

            foreach (int t in v)
            {
                if (t < pivot)
                    left.Add(t);
                else
                    right.Add(t);
            }

            if (T == 1)
            {
                return Unite(QuickSort(left, T), pivot, QuickSort(right, T));
            }
            else
            {
                Task t1 = Task.Factory.StartNew(() => { left = QuickSort(left, T / 2); });
                Task t2 = Task.Factory.StartNew(() => { right = QuickSort(right, T - T / 2); });

                t1.Wait();
                t2.Wait();

                return Unite(left, pivot, right);
            }
        }

        private static void Main(string[] args)
        {
            List<int> result = QuickSort(new List<int> { 9, 8, 7, 6, 5, 4, 3, 2, 1 }, 4);
        }
    }
}