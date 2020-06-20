using System;
using System.Threading;
using System.Threading.Tasks;

namespace myprog
{
    internal class MyClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, world: {0}", String.Join(", ", args));
            Task<long>[] taskArray = new Task<long>[8];
            for (int i = 0; i < 8; ++i)
            {
                int j = i;
                taskArray[i] = Task.Factory.StartNew(() => PartSum(j, 8, 100));
            }

            Task<long> sumTask = Task.Factory.ContinueWhenAll<long, long>(taskArray, FinalSum);
            Console.WriteLine("Everything set up; computations in progress...");
            long sum = sumTask.Result;
            Console.WriteLine("Sum = {0}", sum);
            Console.ReadKey();
        }

        public static long PartSum(int start, int every, int end)
        {
            long sum = 0;
            for (int i = start; i <= end; i += every)
            {
                sum += i;
            }

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