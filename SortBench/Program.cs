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
            new StoogeSort(),
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

            for (int size = 10; size <= 10_000; size += 5)
            {
                Console.WriteLine($"* Running with size {size}");

                // set a limit for maximum number, without it maximum number is 2,147,483,647 so count sort will need 8GB of memory.
                Console.WriteLine("> Generating array...");
                var target = Enumerable.Range(0, size)
                    .Select(i => random.Next(0, 1_000_000))
                    .ToArray();

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

            Console.WriteLine("* Saving plot to plot.svg...");
            _results.SavePlot("plot.svg");
        }
    }
}
