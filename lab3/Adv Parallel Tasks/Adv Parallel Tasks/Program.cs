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

            Matrix m1 = BuildRandomMatrix(10, 10);
            Matrix m2 = BuildRandomMatrix(10, 10);

            Matrix m5 = BuildRandomMatrix(10, 2000000);
            Matrix m6 = BuildRandomMatrix(2000000, 10);


            //            Console.WriteLine(m1);
            //            Console.WriteLine(m2);

            Stopwatch stopWatch = new Stopwatch();
            Console.WriteLine("Starting Operations");
            stopWatch.Start();

//            Matrix result = await Matrix.MatrixSumWithTasksAsync(m1, m2);
//            Matrix result = Matrix.MatrixSumWithThreadPool(m1, m2);
//            Matrix result = Matrix.MatrixSumWithThreads(m1, m2);
//            Matrix result = Matrix.MatrixSumWithTasks(m1, m2);

            //            Matrix result = await Matrix.MatrixMultiplicationWithTasksAsync(m5, m6);
            Matrix result = Matrix.MatrixMultiplicationWithTasks(m5, m6);
            //            Matrix result = Matrix.MatrixMultiplicationWithThreadPool(m5, m6);
            //            Matrix result = Matrix.MatrixMultiplicationWithThreads(m5, m6);
            stopWatch.Stop();

            Console.WriteLine(result);


            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
            Console.WriteLine("RunTime " + elapsedTime);

            Console.ReadKey();
        }
    }
}