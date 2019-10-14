using System;
using System.Collections.Generic;
using System.Threading;

namespace MatrixOperations
{
    class Matrix
    {
        //private int[,] matrixElements;
        public int Row { get; set; }
        public int Column { get; set; }
        private readonly int[,] matrixElements;

        // int[,] array2D = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };

        public Matrix(int[,] matrixElems)
        {
            this.matrixElements = matrixElems;
            Row = matrixElems.GetLength(0);
            Column = matrixElems.GetLength(1);
        }

        public int PutElem(int line, int column, int newValue)
        {
            int oldValue = matrixElements[line, column];
            matrixElements[line, column] = newValue;
            return oldValue;
        }

        public int GetElem(int line, int column)
        {
            return matrixElements[line, column];
        }

        public int GetLength(int dimension)
        {
            return matrixElements.GetLength(dimension);
        }

        public Matrix RandomValues()
        {
            Random rnd = new Random();
            for (int i = 0; i < Row; i++)
            for (int j = 0; j < Column; j++)
                matrixElements[i, j] = rnd.Next(10);
            return this;
        }

        public int[] GetColumn(int i)
        {
            var res = new int[Row];
            for (int j = 0; j < Row; j++)
                res[j] = matrixElements[j, i];
            return res;
        }

        public int[] GetRow(int i)
        {
            var res = new int[Column];
            for (int j = 0; j < Column; j++)
                res[j] = matrixElements[i, j];
            return res;
        }

        public int this[int i, int j]
        {
            get => matrixElements[i, j];
            set => matrixElements[i, j] = value;
        }


        public static Matrix operator *(Matrix a, Matrix b)
        {
            
            var nrThreads = 1;
            Matrix result = new Matrix(new int[a.Row, b.Column]);
            var threads = new List<Thread>();
            for (int i = 0; i < a.Row * b.Column; i++)
            {
                // int tempi = i;
                // Thread thread = new Thread(() => VectorMult(tempi, a, b, result));
                // thread.Start();
                // threads.Add(thread);

            }

            foreach (Thread t in threads)
                t.Join();
            return result;
        }

        // TODO: STUFF
        public static void MultiplyBetweenIndexes(int tmp1, int tmp2, Matrix a, Matrix b, Matrix result)
        {
            for (int tmp = tmp1; tmp < tmp2; tmp++)
            {
                int i = tmp / b.Column;
                int j = tmp % b.Column;
                var x = a.GetRow(i);
                var y = b.GetColumn(j);
                for (int k = 0; k < x.Length; k++)
                    result[i, j] += x[k] * y[k];
            }
        }

        public static void VectorMult(int tmp, Matrix a, Matrix b, Matrix result)
        {
            int i = tmp / b.Column;
            int j = tmp % b.Column;
            var x = a.GetRow(i);
            var y = b.GetColumn(j);
            for (int k = 0; k < x.Length; k++)
                result[i, j] += x[k] * y[k];
            //Console.WriteLine("Calculate element{0}{1}", i, j);
        }


        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < matrixElements.GetLength(0); i++)
            {
                for (int j = 0; j < matrixElements.GetLength(1); j++)
                {
                    s += matrixElements[i,j];
                    s += " ";
                }

                s += "\n";
            }

            return s;
        }
    }
}