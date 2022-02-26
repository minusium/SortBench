using System.Diagnostics;
using SortBench.Algorithms;

namespace SortBench
{
    internal static class Program
    {
        private readonly static ResultContainer _results = new();

        private readonly static ISortAlgorithm[] _algorithms = new ISortAlgorithm[]
        {
            new SelectionSort(),
            new BubbleSort(),
            new QuickSort(),
            new MergeSort(),
            new HeapSort(),
            new InsertionSort(),
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
                    target[i] = random.Next();
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
