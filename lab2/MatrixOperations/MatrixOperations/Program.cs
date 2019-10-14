using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixOperations
{
    class Program
    {
        /*private static Matrix MatrixSum(Matrix a, Matrix b)
        {
            if(a.GetLength(0)!=b.GetLength(0) || a.GetLength(1)!=b.GetLength(1)) throw new Exception("Matrix sizes impossible for summation.");
            Matrix finalMatrix = new Matrix(new int[a.GetLength(0), a.GetLength(1)]);
            for (int i = 0; i < a.GetLength(0); i++)
            for (int j = 0; j < a.GetLength(1); j++)
                finalMatrix.PutElem(i, j, a.GetElem(i, j) + b.GetElem(i, j));
            return finalMatrix;
        }

        private static Matrix MatrixMultiplication(Matrix a, Matrix b)
        {
            if (a.GetLength(1) != b.GetLength(0)) throw new Exception("Matrix sizes impossible for multiplication.");
            Matrix finalMatrix = new Matrix(new int[a.GetLength(0),b.GetLength(1)]);
            for (int i = 0; i < a.GetLength(0); i++)
            {
                // do for every line in matrix a

                for (int j = 0; j < a.GetLength(1); j++)
                {
                    // do for element j in line i
                    //                finalMatrix.PutElem(i, j, a.GetElem(i, j) + b.GetElem(i, j));




                }
            }


            return finalMatrix;
        }*/

//        1,2
//        3,4
//        5,6
//        7,8
//        2,1
//
//        1,2,3,4
//        5,6,7,8


        static void Main(string[] args)
        {
            /*Matrix m1 = new Matrix(new int[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}});
            Matrix m2 = new Matrix(new int[,] {{3, 2, 1}, {6, 5, 4}, {3, 3, 3}});
            Matrix m3 = new Matrix(new int[,] {{1, 2}, {3, 4}, {5, 6}, {7, 8}, {2, 1}});
            Matrix m4 = new Matrix(new int[,] {{1, 2, 3, 4}, {5, 6, 7, 8}});
            Console.WriteLine(m1+"\n"+m2+"\n"+m3+"\n"+m4);

            Console.WriteLine(m1 + "+\n" + m2 +"=");
            Console.WriteLine(MatrixSum(m1, m2).ToString());
            Console.WriteLine(MatrixMultiplication(m3, m4).ToString());*/

            Console.WriteLine("Input number of rows for matrix 1");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine("Input number of columns for matrix 1");
            int m = int.Parse(Console.ReadLine());
            Console.WriteLine("Input number of columns for matrix 2");
            int k = int.Parse(Console.ReadLine());

            Matrix A = new Matrix(new int[n, m]).RandomValues();
            Matrix B = new Matrix(new int[m, k]).RandomValues();

            Console.WriteLine(A.ToString());
            Console.WriteLine(new String('-', 20));
            Console.WriteLine(B.ToString());
            Console.WriteLine(new String('-', 20));
            Matrix C = A * B;
            Console.WriteLine(C.ToString());


            Console.ReadKey();
        }




        /*class Program
        {
            static void Main(string[] args)
            {
                int n = int.Parse(Console.ReadLine());
                int m = int.Parse(Console.ReadLine());
                int k = int.Parse(Console.ReadLine());
                Matrix A = new Matrix(n, m).RandomValues();
                Matrix B = new Matrix(m, k).RandomValues();
                A.Print();
                Console.WriteLine(new String('-', 20));
                B.Print();
                Console.WriteLine(new String('-', 20));
                Matrix C = A * B;
                C.Print();


                Console.ReadKey();
        }
        }*/



    }



}
