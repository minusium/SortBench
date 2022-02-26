using System.Diagnostics;
using SortBench.Algorithms;

namespace SortBench
{
    internal static class Program
    {
        private readonly static ResultContainer _results = new();

        private readonly static ISortAlgorithm[] _algorithms = new ISortAlgorithm[]
        {
            new BubbleSort(),
            new InsertionSort(),
            new SelectionSort(),
            new QuickSort(),
            new MergeSort(),
            new CountSort(),
            new RadixSort(),
            new HeapSort(),
        };

        private static long BenchAlgorithm(this ISortAlgorithm algorithm, int[] target, int[] sorted)
        {
            var stopwatch = new Stopwatch();
            var clonedTarget = new int[target.Length];
            Array.Copy(target, clonedTarget, clonedTarget.Length);
            stopwatch.Start();
            algorithm.Run(clonedTarget);
            stopwatch.Stop();
            for (int i = 0; i < target.Length; i++)
            {
                Debug.Assert(clonedTarget[i] == sorted[i]);
            }
            return stopwatch.ElapsedTicks;
        }

        public static void Main()
        {
            foreach (var algorithm in _algorithms)
            {
                _results.AddColumn(algorithm.Name);
            }

            var random = new Random();

            for (int size = 1000; size <= 50000; size += 500)
            {
                Console.WriteLine($"* Running with size {size}");

                Console.WriteLine("> Generating array...");
                var target = new int[size];
                for (int i = 0; i < size; i++)
                {
                    // with 128,000,000 as maximum number count sort will require 488MB memory so my system won't crash
                    // without limit, maximum number is 2,147,483,647 so count sort will need 8GB of memory.
                    target[i] = random.Next(0, 128_000_000);
                }

                var answer = target.OrderBy(i => i).ToArray();

                foreach (var algorithm in _algorithms)
                {
                    Console.WriteLine($"> Running {algorithm.Name}...");
                    var elapsed = algorithm.BenchAlgorithm(target, answer);
                    Console.WriteLine($"took {elapsed} ticks.");
                    _results.AddResult(size, algorithm.Name, elapsed);
                }

                Console.WriteLine();
            }
            Console.WriteLine("* Saving result to result.csv...");
            _results.SaveAsCsv("result.csv");
        }
    }
}
