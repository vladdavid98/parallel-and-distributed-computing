using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixOperations
{
    // comparatie threadpool, async, task
    internal class Matrix
    {
        public Matrix(int[,] newArr)
        {
            Array2D = newArr;
            NrRows = newArr.GetLength(0);
            NrColumns = newArr.GetLength(1);
        }

        public int[,] Array2D { get; set; }

        public int NrRows { get; set; }

        public int NrColumns { get; set; }

        public int[] GetRow(int rowNr)
        {
            var result = new int[NrColumns];
            for (var i = 0; i < NrColumns; i++)
                result[i] = Array2D[rowNr, i];
            return result;
        }

        public int[] GetColumn(int columnNr)
        {
            var result = new int[NrRows];
            for (var i = 0; i < NrRows; i++)
                result[i] = Array2D[i, columnNr];
            return result;
        }

        public override string ToString()
        {
            var s = "";
            for (var i = 0; i < NrRows; i++)
            {
                for (var j = 0; j < NrColumns; j++)
                {
                    s += Array2D[i, j];
                    s += " ";
                }

                s += Environment.NewLine;
            }

            return s;
        }

        private static int HowManyZeroes(int[] arr)
        {
            return arr.Count(i => i == 0);
        }


        public static Matrix TripleMatrixMultiplication(Matrix m1, Matrix m2, Matrix m3)
        {
            var intermediaryMatrix = new Matrix(new int[m1.NrRows, m2.NrColumns]);
            var finalResultMatrix = new Matrix(new int[intermediaryMatrix.NrRows, m3.NrColumns]);
            var columnsInFinalMatrixToBeComputed = Enumerable.Repeat(0, finalResultMatrix.NrColumns).ToArray();

            var tasks = new List<Task>();
            // first multiply matrices m1 and m2 row by row. 
            // resulting matrix of this first operation will be called intermediaryMatrix.

            // after first row of intermediaryMatrix is computed, computation of first column of finalResultMatrix is possible.

            var autoResetEvent = new AutoResetEvent(false);
            for (var r = 0; r < intermediaryMatrix.NrColumns; r++)
            {
                var r1 = r;
                Task task = Task.Factory.StartNew(async () =>
                {
                    await GetMultiplicationResultForRow(intermediaryMatrix, m1, m2, r1);
                });
                tasks.Add(task);
                columnsInFinalMatrixToBeComputed[r1] = 1;
                foreach (var i in columnsInFinalMatrixToBeComputed) Console.Write(i + " ");
                Console.WriteLine();
            }


            //            while (HowManyZeroes(columnsInFinalMatrixToBeComputed) != 0)
            //            {
            //                foreach (var i in columnsInFinalMatrixToBeComputed)
            //                {
            //                    
            //                }
            //                
            //            }

            int numberOfCurrentCompletedTasks;
            do
            {
                numberOfCurrentCompletedTasks = 0;
                foreach (var t in tasks)
                    if (t.IsCompleted)
                    {
                        numberOfCurrentCompletedTasks++;
                        Console.WriteLine("kaka");
                    }
            } while (numberOfCurrentCompletedTasks != tasks.Count);

            //            Thread.Sleep(1000);
            foreach (var t in tasks) t.Wait();
            return intermediaryMatrix;
        }

        public static Matrix MatrixMultiplicationWithThreads(Matrix m1, Matrix m2)
        {
            var nrOfThreads = Program.NrThreads;
            var threads = new List<Thread>();

            if (m1.NrColumns != m2.NrRows)
                throw new Exception("Impossible to multiply, due to wrong matrix sizes.");

            var mResult = new Matrix(new int[m1.NrRows, m2.NrColumns]);

            var totalElems = mResult.NrRows * mResult.NrColumns;
            var nrOfOperationsPerThread = totalElems / nrOfThreads;

            var starti = 0;
            var startj = 0;
            var endi = 0;
            var endj = 0;

            for (var i = 1; i <= totalElems; i++)
            {
                if (endj == mResult.NrColumns)
                {
                    endi++;
                    endj = 0;
                }

                if (i % nrOfOperationsPerThread == 0)
                {
                    int[] sp = {starti, startj};
                    int[] ep = {endi, endj};
                    threads.Add(new Thread(() => { GetMultiplicationResultForPosition(mResult, m1, m2, sp, ep); }));
                    starti = endi;
                    startj = endj;
                }

                if (i > nrOfOperationsPerThread * nrOfThreads)
                {
                    int[] sp = {starti, startj};
                    int[] ep = {mResult.NrRows - 1, mResult.NrColumns - 1};
                    threads.Add(new Thread(() => { GetMultiplicationResultForPosition(mResult, m1, m2, sp, ep); }));
                    i = totalElems;
                }

                endj++;
            }

            foreach (var t in threads) t.Start();

            foreach (var t in threads) t.Join();

            return mResult;
        }


        public static Matrix MatrixMultiplicationWithThreadPool(Matrix m1, Matrix m2)
        {
            var nrOfThreads = Program.NrThreads;
            if (m1.NrColumns != m2.NrRows)
                throw new Exception("Impossible to multiply, due to wrong matrix sizes.");

            var mResult = new Matrix(new int[m1.NrRows, m2.NrColumns]);

            var totalElems = mResult.NrRows * mResult.NrColumns;
            var nrOfOperationsPerThread = totalElems / nrOfThreads;
            var starti = 0;
            var startj = 0;
            var endi = 0;
            var endj = 0;
            var countdown = new CountdownEvent(nrOfThreads);
            var j = 0;
            j = 0;
            for (var i = 1; i <= totalElems; i++)
            {
                if (endj == mResult.NrColumns)
                {
                    endi++;
                    endj = 0;
                }

                if (i % nrOfOperationsPerThread == 0)
                {
                    int[] sp = {starti, startj};
                    int[] ep = {endi, endj};

                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        j++;
                        await GetMultiplicationResultForPosition(mResult, m1, m2, sp, ep);
                        countdown.Signal();
                    });
                    starti = endi;
                    startj = endj;
                }

                if (i > nrOfOperationsPerThread * nrOfThreads)
                {
                    int[] sp = {starti, startj};
                    int[] ep = {mResult.NrRows - 1, mResult.NrColumns - 1};
                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        j++;
                        await GetMultiplicationResultForPosition(mResult, m1, m2, sp, ep);
                        countdown.Signal();
                    });
                    i = totalElems;
                }

                endj++;
            }

            countdown.Wait();

            return mResult;
        }


        public static Matrix MatrixMultiplicationWithTasks(Matrix m1, Matrix m2)
        {
            var tasks = new List<Task>();
            var nrOfThreads = Program.NrThreads;
            if (m1.NrColumns != m2.NrRows)
                throw new Exception("Impossible to multiply, due to wrong matrix sizes.");

            var mResult = new Matrix(new int[m1.NrRows, m2.NrColumns]);

            var totalElems = mResult.NrRows * mResult.NrColumns;
            var nrOfOperationsPerThread = totalElems / nrOfThreads;

            var starti = 0;
            var startj = 0;
            var endi = 0;
            var endj = 0;

            for (var i = 1; i <= totalElems; i++)
            {
                if (endj == mResult.NrColumns)
                {
                    endi++;
                    endj = 0;
                }

                if (i % nrOfOperationsPerThread == 0)
                {
                    int[] sp = {starti, startj};
                    int[] ep = {endi, endj};
                    var t = Task.Run(() => GetMultiplicationResultForPosition(mResult, m1, m2, sp, ep));
                    tasks.Add(t);
                    starti = endi;
                    startj = endj;
                }

                if (i > nrOfOperationsPerThread * nrOfThreads)
                {
                    int[] sp = {starti, startj};
                    int[] ep = {mResult.NrRows - 1, mResult.NrColumns - 1};
                    var t = Task.Run(() => GetMultiplicationResultForPosition(mResult, m1, m2, sp, ep));
                    tasks.Add(t);
                    i = totalElems;
                }

                endj++;
            }

            foreach (var t in tasks) t.Wait();
            return mResult;
        }


        public static async Task<Matrix> MatrixMultiplicationWithTasksAsync(Matrix m1, Matrix m2)
        {
            var nrOfThreads = Program.NrThreads;
            if (m1.NrColumns != m2.NrRows)
                throw new Exception("Impossible to multiply, due to wrong matrix sizes.");

            var mResult = new Matrix(new int[m1.NrRows, m2.NrColumns]);

            var totalElems = mResult.NrRows * mResult.NrColumns;
            var nrOfOperationsPerThread = totalElems / nrOfThreads;

            var starti = 0;
            var startj = 0;
            var endi = 0;
            var endj = 0;
            for (var i = 1; i <= totalElems; i++)
            {
                if (endj == mResult.NrColumns)
                {
                    endi++;
                    endj = 0;
                }

                if (i % nrOfOperationsPerThread == 0)
                {
                    int[] sp = {starti, startj};
                    int[] ep = {endi, endj};
                    await Task.Run(() => GetMultiplicationResultForPosition(mResult, m1, m2, sp, ep));
                    starti = endi;
                    startj = endj;
                }

                if (i > nrOfOperationsPerThread * nrOfThreads)
                {
                    int[] sp = {starti, startj};
                    int[] ep = {mResult.NrRows - 1, mResult.NrColumns - 1};
                    await Task.Run(() => GetMultiplicationResultForPosition(mResult, m1, m2, sp, ep));
                    i = totalElems;
                }

                endj++;
            }

            return mResult;
        }

        public static async Task GetMultiplicationResultForRow(Matrix mResult, Matrix m1, Matrix m2, int rowNumber)
        {
            var i = rowNumber;
            var j = 0;
            var k = rowNumber;
            var l = mResult.NrColumns - 1;
            int[] m1Row;
            int[] m2Column;

            while (!(i == k && j == l))
            {
                m1Row = m1.GetRow(i);
                m2Column = m2.GetColumn(j);
                mResult.Array2D[i, j] = m1Row.Select((element, index) => element * m2Column[index]).Sum();
                j++;
                if (j == mResult.NrColumns)
                {
                    i++;
                    j = 0;
                }
            }

            if (i == k && j == l)
            {
                m1Row = m1.GetRow(i);
                m2Column = m2.GetColumn(j);
                mResult.Array2D[i, j] = m1Row.Select((element, index) => element * m2Column[index]).Sum();
            }
        }

        /*public static async Task GetMultiplicationResultForColumn(Matrix mResult, Matrix m1, Matrix m2, int colNumber)
        {
            int i = rowNumber;
            int j = 0;
            int k = rowNumber;
            int l = mResult.NrColumns - 1;
            int[] m1Row;
            int[] m2Column;

            while (!(i == k && j == l))
            {
                m1Row = m1.GetRow(i);
                m2Column = m2.GetColumn(j);
                mResult.Array2D[i, j] = m1Row.Select((element, index) => element * m2Column[index]).Sum();
                j++;
                if (j == mResult.NrColumns)
                {
                    i++;
                    j = 0;
                }
            }

            if (i == k && j == l)
            {
                m1Row = m1.GetRow(i);
                m2Column = m2.GetColumn(j);
                mResult.Array2D[i, j] = m1Row.Select((element, index) => element * m2Column[index]).Sum();
            }
        }*/


        private static async Task GetMultiplicationResultForPosition(Matrix mResult, Matrix m1, Matrix m2,
            int[] startingPoint,
            int[] endingPoint)
        {
            var i = startingPoint[0];
            var j = startingPoint[1];
            var k = endingPoint[0];
            var l = endingPoint[1];
            int[] m1Row;
            int[] m2Column;

            while (!(i == k && j == l))
            {
                m1Row = m1.GetRow(i);
                m2Column = m2.GetColumn(j);
                mResult.Array2D[i, j] = m1Row.Select((element, index) => element * m2Column[index]).Sum();
                j++;
                if (j == mResult.NrColumns)
                {
                    i++;
                    j = 0;
                }
            }

            if (i == k && j == l)
            {
                m1Row = m1.GetRow(i);
                m2Column = m2.GetColumn(j);
                mResult.Array2D[i, j] = m1Row.Select((element, index) => element * m2Column[index]).Sum();
            }
        }
    }
}