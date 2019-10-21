using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixOperations
{
    internal class Program
    {
        public static int NrThreads = 4;
        public static Random Rnd = new Random(1);

        private static Matrix BuildRandomMatrix(int nrRows, int nrColumns)
        {
            var array2D = new int[nrRows, nrColumns];
            for (int i = 0; i < nrRows; i++)
            for (int j = 0; j < nrColumns; j++)
                array2D[i, j] = Rnd.Next(0, 10);
            return new Matrix(array2D);
        }

        private static async Task Main(string[] args)
        {
            //            Matrix m1 = BuildRandomMatrix(15000, 10000);
            //            Matrix m2 = BuildRandomMatrix(15000, 10000);
            //            Matrix m3 = BuildRandomMatrix(10, 2000000);
            //            Matrix m4 = BuildRandomMatrix(2000000, 10);

            Matrix m5 = BuildRandomMatrix(10, 2000000);
            Matrix m6 = BuildRandomMatrix(2000000, 10);
            //            Console.WriteLine(m1);
            //            Console.WriteLine(m2);

            Stopwatch stopWatch = new Stopwatch();
            Console.WriteLine("Starting Multiplication");
            stopWatch.Start();
            //            Matrix result1 = Matrix.MatrixSumWithThreadPool(m1, m2);
            //            Matrix result2 = Matrix.MatrixMultiplicationWithTasks(m3, m4);

            Matrix result3 = await Matrix.MatrixMultiplicationWithTasksAsync(m5, m6);

            stopWatch.Stop();

            Console.WriteLine(result3);


            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            Console.ReadKey();
        }
    }
}