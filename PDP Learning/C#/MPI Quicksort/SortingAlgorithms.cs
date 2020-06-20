using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPI_Quicksort
{
    internal class SortingAlgorithms
    {
        public static void ParallelQuickSort(List<int> arr, int lo, int hi)
        {
            if (hi - lo < 100)
            {
                QuickSort(arr, lo, hi);
            }
            else
            {
                int p = HoarePartition(arr, lo, hi);
                Parallel.Invoke(
                    () => ParallelQuickSort(arr, lo, p),
                    () => ParallelQuickSort(arr, p + 1, hi)
                );
            }
        }

        public static void swap(List<int> arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        private static int HoarePartition(List<int> arr, int low, int high)
        {
            int pivotPoint = low + (high - low) / 2;
            int pivot = arr[pivotPoint];
            while (true)
            {
                while (arr[low] < pivot)
                {
                    low++;
                }

                while (pivot < arr[high])
                {
                    high--;
                }

                if (low >= high) return high;
                swap(arr, low, high);
                low++;
                high--;
            }
        }

        private static void InsertionSort(List<int> arr, int lo, int hi)
        {
            /* To deal with small arrays
            ** Loop through the array from the second element, insert the element to the correct position
            **
            */
            for (int i = lo + 1; i <= hi; i++)
            {
                int j = i - 1;
                int x = arr[i];
                while (j >= 0 && arr[j].CompareTo(x) > 0)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }

                arr[j + 1] = x;
            }
        }

        public static void QuickSort(List<int> arr, int lo, int hi)
        {
            /*
            ** QuickSort an array, if the array length is less than 40, use InsertionSort instead.
            **
            */

            if (hi - lo < 40)
            {
                InsertionSort(arr, lo, hi);
            }
            else
            {
                int p = HoarePartition(arr, lo, hi);
                QuickSort(arr, lo, p);
                QuickSort(arr, p + 1, hi);
            }
        }
    }
}