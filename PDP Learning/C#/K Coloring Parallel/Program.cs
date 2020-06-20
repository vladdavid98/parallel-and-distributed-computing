using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Coloring_Parallel
{
    internal class Program
    {
        private bool check(List<int> colour)
        {
            return true;
        }

        private List<int> to_color(int sol, int n, int k)
        {
            List<int> v = new List<int>();
            for (int i = 0; i < n; i++)
            {
                v.Add(sol % k);
                sol /= k;
            }

            return v;
        }

        private List<int> solve(int n, int k, int T)
        {
            int maxi = 1;
            for (int i = 0; i < n; i++)
            {
                maxi *= k;
            }

            List<int> sol = new List<int>();

            for (int t = 0; t < T; i++)
            {
            }
        }

        private static void Main(string[] args)
        {
        }
    }
}