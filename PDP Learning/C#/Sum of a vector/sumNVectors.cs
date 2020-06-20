using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sum_n_vectors
{
    internal class Program
    {
        public static List<List<int>> ListOfLists = new List<List<int>>();

        private static void Main(string[] args)
        {
            Random r = new Random();
            int rInt; //for ints
            long s = 0; // sum, for verifying purposes
            int nrOfThreads = 8;

            int n = 5; // number of lists
            int m = 10; // number of elements inside each list

            for (int i = 0; i < n; i++)
            {
                var newList = new List<int>();
                for (int j = 0; j < m; j++)
                {
                    rInt = r.Next(0, 100);
                    newList.Add(rInt);
                    s += rInt;
                }

                ListOfLists.Add(newList);
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(ListOfLists[i][j] + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Sum: {0}", s);

            Task<long>[] taskArray = new Task<long>[nrOfThreads];
            for (int i = 0; i < nrOfThreads; ++i)
            {
                int j = i;
                taskArray[i] = Task.Factory.StartNew(() => PartSum(j, nrOfThreads, ListOfLists[0].Count));
            }

            Task<long> sumTask = Task.Factory.ContinueWhenAll<long, long>(taskArray, FinalSum);

            long finalSum = sumTask.Result;
            Console.WriteLine("Sum calculated with tasks = {0}", finalSum);

            Console.ReadKey();
        }

        public static long PartSum(int startIndex, int every, int endIndex)
        {
            long sum = 0;
            for (int i = startIndex; i < endIndex; i += every) // i is the column numbers that will be summed
            {
                for (int j = 0;
                    j < ListOfLists.Count;
                    j++) // j goes from 0 to the number of rows (rows are lists of int)
                {
                    sum += ListOfLists[j][i];
                }
            }

            Console.WriteLine("Partial sum for startindex {1}: {0}", sum, startIndex);

            return sum;
        }

        public static long FinalSum(Task<long>[] taskArray)
        {
            Console.WriteLine("Partial sums computed");
            long sum = 0;
            foreach (Task<long> t in taskArray)
            {
                sum += t.Result;
            }

            return sum;
        }
    }
}