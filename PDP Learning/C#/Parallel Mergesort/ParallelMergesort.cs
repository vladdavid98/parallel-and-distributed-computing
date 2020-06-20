using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_Mergesort
{
    // WORKS PERFECTLY
    internal class ParallelMergesort
    {
        private static void Main(string[] args)
        {
            int[] arr = { 5, 3, 1, 7, 8, 5, 3, 2, 6, 7, 9, 3, 2, 4, 2, 1 };
            Sort(arr, 0, arr.Length, new int[arr.Length]);
        }

        private static void Sort(int[] arr, int leftPos, int rightPos, int[] tempArr)
        {
            int len = rightPos - leftPos;
            if (len < 2)
                return;
            if (len == 2)
            {
                if (arr[leftPos] > arr[leftPos + 1])
                {
                    int t = arr[leftPos];
                    arr[leftPos] = arr[leftPos + 1];
                    arr[leftPos + 1] = t;
                }

                return;
            }

            int rStart = leftPos + len / 2;

            Parallel.Invoke(
                () => Sort(arr, leftPos, rStart, tempArr),
                () => Sort(arr, rStart, rightPos, tempArr)
            );

            int l = leftPos;
            int r = rStart;
            int z = leftPos;
            while (l < rStart && r < rightPos)
            {
                if (arr[l] < arr[r])
                {
                    tempArr[z] = arr[l];
                    l++;
                }
                else
                {
                    tempArr[z] = arr[r];
                    r++;
                }

                z++;
            }

            if (l < rStart)
                Array.Copy(arr, l, tempArr, z, rStart - l);
            else
                Array.Copy(arr, r, tempArr, z, rightPos - r);
            Array.Copy(tempArr, leftPos, arr, leftPos, rightPos - leftPos);
        }
    }
}