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
            for (int i = 0; i < NrColumns; i++)
                result[i] = Array2D[rowNr, i];
            return result;
        }

        public int[] GetColumn(int columnNr)
        {
            var result = new int[NrRows];
            for (int i = 0; i < NrRows; i++)
                result[i] = Array2D[i, columnNr];
            return result;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < NrRows; i++)
            {
                for (int j = 0; j < NrColumns; j++)
                {
                    s += Array2D[i, j];
                    s += " ";
                }

                s += Environment.NewLine;
            }

            return s;
        }

        public static Matrix MatrixSumWithThreads(Matrix m1, Matrix m2)
        {
            int nrOfThreads = Program.NrThreads;
            var threads = new List<Thread>();

            if (m1.NrRows != m2.NrRows || m1.NrColumns != m2.NrColumns)
                throw new Exception("Impossible to add, due to wrong matrix sizes.");

            Matrix mResult = new Matrix(new int[m1.NrRows, m2.NrColumns]);

            int totalElems = mResult.NrRows * mResult.NrColumns;
            int nrOfOperationsPerThread = totalElems / nrOfThreads;

            int starti = 0;
            int startj = 0;
            int endi = 0;
            int endj = 0;

            for (int i = 1; i <= totalElems; i++)
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
                    threads.Add(new Thread(() => { GetSumResultForPosition(mResult, m1, m2, sp, ep); }));
                    starti = endi;
                    startj = endj;
                }

                if (i > nrOfOperationsPerThread * nrOfThreads)
                {
                    int[] sp = {starti, startj};
                    int[] ep = {mResult.NrRows - 1, mResult.NrColumns - 1};
                    threads.Add(new Thread(() => { GetSumResultForPosition(mResult, m1, m2, sp, ep); }));
                    i = totalElems;
                }

                endj++;
            }

            foreach (Thread t in threads) t.Start();

            foreach (Thread t in threads) t.Join();

            return mResult;
        }

        public static Matrix MatrixSumWithThreadPool(Matrix m1, Matrix m2)
        {
            int nrOfThreads = Program.NrThreads;

            if (m1.NrRows != m2.NrRows || m1.NrColumns != m2.NrColumns)
                throw new Exception("Impossible to add, due to wrong matrix sizes.");

            Matrix mResult = new Matrix(new int[m1.NrRows, m2.NrColumns]);

            int totalElems = mResult.NrRows * mResult.NrColumns;
            int nrOfOperationsPerThread = totalElems / nrOfThreads;

            int starti = 0;
            int startj = 0;
            int endi = 0;
            int endj = 0;
            var events = new ManualResetEvent[nrOfThreads];
            int j = 0;

            for (int i = 1; i <= totalElems; i++)
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
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        events[j] = new ManualResetEvent(false);
                        events[j].Set();
                        j++;
                        GetSumResultForPosition(mResult, m1, m2, sp, ep);
                    });

                    starti = endi;
                    startj = endj;
                }

                if (i > nrOfOperationsPerThread * nrOfThreads)
                {
                    int[] sp = {starti, startj};
                    int[] ep = {mResult.NrRows - 1, mResult.NrColumns - 1};
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        events[j] = new ManualResetEvent(false);
                        events[j].Set();
                        j++;
                        GetSumResultForPosition(mResult, m1, m2, sp, ep);
                    });
                    i = totalElems;
                }

                endj++;
            }

            try
            {
                WaitHandle.WaitAll(events);
            }
            catch
            {
            }

            return mResult;
        }

        public static async Task<Matrix> MatrixSumWithTasksAsync(Matrix m1, Matrix m2)
        {
            int nrOfThreads = Program.NrThreads;
            var threads = new List<Thread>();

            if (m1.NrRows != m2.NrRows || m1.NrColumns != m2.NrColumns)
                throw new Exception("Impossible to add, due to wrong matrix sizes.");

            Matrix mResult = new Matrix(new int[m1.NrRows, m2.NrColumns]);

            int totalElems = mResult.NrRows * mResult.NrColumns;
            int nrOfOperationsPerThread = totalElems / nrOfThreads;

            int starti = 0;
            int startj = 0;
            int endi = 0;
            int endj = 0;

            for (int i = 1; i <= totalElems; i++)
            {
                if (endj == mResult.NrColumns)
                {
                    endi++;
                    endj = 0;
                }

                if (i % nrOfOperationsPerThread == 0)
                {
                    int[] sp = { starti, startj };
                    int[] ep = { endi, endj };
                    await Task.Run(() => { GetSumResultForPosition(mResult, m1, m2, sp, ep); });
                    starti = endi;
                    startj = endj;
                }

                if (i > nrOfOperationsPerThread * nrOfThreads)
                {
                    int[] sp = { starti, startj };
                    int[] ep = { mResult.NrRows - 1, mResult.NrColumns - 1 };
                    await Task.Run(() => { GetSumResultForPosition(mResult, m1, m2, sp, ep); });
                    i = totalElems;
                }

                endj++;
            }

            return mResult;
        }


        public static Matrix MatrixMultiplicationWithThreads(Matrix m1, Matrix m2)
        {
            int nrOfThreads = Program.NrThreads;
            var threads = new List<Thread>();

            if (m1.NrColumns != m2.NrRows)
                throw new Exception("Impossible to multiply, due to wrong matrix sizes.");

            Matrix mResult = new Matrix(new int[m1.NrRows, m2.NrColumns]);

            int totalElems = mResult.NrRows * mResult.NrColumns;
            int nrOfOperationsPerThread = totalElems / nrOfThreads;

            int starti = 0;
            int startj = 0;
            int endi = 0;
            int endj = 0;

            for (int i = 1; i <= totalElems; i++)
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

            foreach (Thread t in threads) t.Start();

            foreach (Thread t in threads) t.Join();

            return mResult;
        }


        public static Matrix MatrixMultiplicationWithThreadPool(Matrix m1, Matrix m2)
        {
            int nrOfThreads = Program.NrThreads;
            if (m1.NrColumns != m2.NrRows)
                throw new Exception("Impossible to multiply, due to wrong matrix sizes.");

            Matrix mResult = new Matrix(new int[m1.NrRows, m2.NrColumns]);

            int totalElems = mResult.NrRows * mResult.NrColumns;
            int nrOfOperationsPerThread = totalElems / nrOfThreads;

            int starti = 0;
            int startj = 0;
            int endi = 0;
            int endj = 0;
            CountdownEvent countdown = new CountdownEvent(nrOfThreads);

            int j = 0;

            j = 0;
            for (int i = 1; i <= totalElems; i++)
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
            int nrOfThreads = Program.NrThreads;
            if (m1.NrColumns != m2.NrRows)
                throw new Exception("Impossible to multiply, due to wrong matrix sizes.");

            Matrix mResult = new Matrix(new int[m1.NrRows, m2.NrColumns]);

            int totalElems = mResult.NrRows * mResult.NrColumns;
            int nrOfOperationsPerThread = totalElems / nrOfThreads;

            int starti = 0;
            int startj = 0;
            int endi = 0;
            int endj = 0;

            for (int i = 1; i <= totalElems; i++)
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
                    Task t = Task.Run(() => GetMultiplicationResultForPosition(mResult, m1, m2, sp, ep));
                    tasks.Add(t);
                    starti = endi;
                    startj = endj;
                }

                if (i > nrOfOperationsPerThread * nrOfThreads)
                {
                    int[] sp = {starti, startj};
                    int[] ep = {mResult.NrRows - 1, mResult.NrColumns - 1};
                    Task t = Task.Run(() => GetMultiplicationResultForPosition(mResult, m1, m2, sp, ep));
                    tasks.Add(t);
                    i = totalElems;
                }

                endj++;
            }

            foreach (Task t in tasks) t.Wait();
            return mResult;
        }


        public static async Task<Matrix> MatrixMultiplicationWithTasksAsync(Matrix m1, Matrix m2)
        {
            int nrOfThreads = Program.NrThreads;
            if (m1.NrColumns != m2.NrRows)
                throw new Exception("Impossible to multiply, due to wrong matrix sizes.");

            Matrix mResult = new Matrix(new int[m1.NrRows, m2.NrColumns]);

            int totalElems = mResult.NrRows * mResult.NrColumns;
            int nrOfOperationsPerThread = totalElems / nrOfThreads;

            int starti = 0;
            int startj = 0;
            int endi = 0;
            int endj = 0;

            for (int i = 1; i <= totalElems; i++)
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

        private static async Task GetSumResultForPosition(Matrix mResult, Matrix m1, Matrix m2, int[] startingPoint,
            int[] endingPoint)
        {
            int i = startingPoint[0];
            int j = startingPoint[1];
            int k = endingPoint[0];
            int l = endingPoint[1];

            while (!(i == k && j == l))
            {
                mResult.Array2D[i, j] = m1.Array2D[i, j] + m2.Array2D[i, j];
                j++;
                if (j == mResult.NrColumns)
                {
                    i++;
                    j = 0;
                }
            }

            if (i == k && j == l) mResult.Array2D[i, j] = m1.Array2D[i, j] + m2.Array2D[i, j];
        }

        private static async Task GetMultiplicationResultForPosition(Matrix mResult, Matrix m1, Matrix m2,
            int[] startingPoint,
            int[] endingPoint)
        {
            int i = startingPoint[0];
            int j = startingPoint[1];
            int k = endingPoint[0];
            int l = endingPoint[1];
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