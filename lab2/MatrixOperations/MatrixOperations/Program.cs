using System;
using System.Diagnostics;

namespace MatrixOperations
{
    internal class Program
    {
        public static int nrThreads = 4;
        public static Random Rnd = new Random(2);

        private static Matrix BuildRandomMatrix(int nrRows, int nrColumns)
        {
            var array2D = new int[nrRows, nrColumns];
            for (int i = 0; i < nrRows; i++)
            for (int j = 0; j < nrColumns; j++)
                array2D[i, j] = Rnd.Next(0, 10);
            return new Matrix(array2D);
        }

        private static void Main(string[] args)
        {
//            Matrix m1 = BuildRandomMatrix(10000, 10000);
//            Matrix m2 = BuildRandomMatrix(10000, 10000);
            Matrix m3 = BuildRandomMatrix(300, 300);
            Matrix m4 = BuildRandomMatrix(300, 300);
            //            Console.WriteLine(m1);
            //            Console.WriteLine(m2);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
//            Matrix result1 = Matrix.MatrixSum(m1, m2);
            Matrix result2 = Matrix.MatrixMultiplication(m3, m4);
            stopWatch.Stop();

//            Console.WriteLine(result2);


            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
            Console.WriteLine("RunTime " + elapsedTime);

            Console.ReadKey();
        }
    }
}