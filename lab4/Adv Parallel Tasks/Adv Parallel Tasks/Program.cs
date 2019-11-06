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
            Matrix m1 = BuildRandomMatrix(5, 5);
            Matrix m2 = BuildRandomMatrix(5, 5);
            Matrix m3 = BuildRandomMatrix(5, 5);
            Console.WriteLine(m1);
            Console.WriteLine(m2);
            Console.WriteLine(m3);

            Stopwatch stopWatch = new Stopwatch();
            Console.WriteLine("Starting Operations");
            stopWatch.Start();


//            Matrix result =  Matrix.MatrixMultiplicationWithTasks(m1,m2);

            Matrix result = Matrix.TripleMatrixMultiplication(m1,m2,m3);

            stopWatch.Stop();

            Console.WriteLine(result);


            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
            Console.WriteLine("RunTime " + elapsedTime);

            Console.ReadKey();
        }
    }
}