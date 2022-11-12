using System;
using System.Diagnostics;
using System.Linq;

using SortBench.Core.Algorithms;

namespace SortBench.Core
{
    public interface ISortAlgorithm
    {
        string Name { get; }

        void Run(int[] target);

        ulong CalculateRequiredMemory(uint maxSize, int maxValue);
    }

    public static class SortAlgorithms
    {
        public static readonly ISortAlgorithm[] All = {
            new BubbleSort(),
            new InsertionSort(),
            new SelectionSort(),
            new QuickSort(),
            new MergeSort(),
            new CountSort(),
            new RadixSort(),
            new HeapSort(),
        };

        public static long Benchmark(this ISortAlgorithm algorithm, int[] target)
        {
            var stopwatch = new Stopwatch();

            // clone the array before sorting
            var clonedTarget = new int[target.Length];
            Array.Copy(target, clonedTarget, clonedTarget.Length);

            // run the sort algorithm and calculate elapsed time
            stopwatch.Start();
            algorithm.Run(clonedTarget);
            stopwatch.Stop();

#if DEBUG
            // verify that the array is sorted correctly
            var sorted = target.OrderBy(i => i).ToArray();
            for (var i = 0; i < target.Length; i++)
            {
                Debug.Assert(clonedTarget[i] == sorted[i]);
            }
#endif

            return stopwatch.ElapsedTicks;
        }
    }
}
