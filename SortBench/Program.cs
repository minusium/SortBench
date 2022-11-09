using System.Diagnostics;
using SortBench.Algorithms;

namespace SortBench
{
    internal static class Program
    {
        private static readonly ResultContainer Results = new();

        private static readonly ISortAlgorithm[] Algorithms = {
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

        private static long BenchAlgorithm(this ISortAlgorithm algorithm, int[] target
#if DEBUG
            , IReadOnlyList<int> sorted
#endif
            )
        {
            var stopwatch = new Stopwatch();
            var clonedTarget = new int[target.Length];
            Array.Copy(target, clonedTarget, clonedTarget.Length);
            stopwatch.Start();
            algorithm.Run(clonedTarget);
            stopwatch.Stop();
#if DEBUG
            for (var i = 0; i < target.Length; i++)
            {
                Debug.Assert(clonedTarget[i] == sorted[i]);
            }
#endif
            return stopwatch.ElapsedTicks;
        }

        public static void Main()
        {
            foreach (var algorithm in Algorithms)
            {
                Results.AddColumn(algorithm.Name);
            }

            var random = new Random();

            for (int size = 10; size <= 10_000; size += 5)
            {
                Console.WriteLine($"* Running with size {size}");

                // set a limit for maximum number, without it maximum number is 2,147,483,647 so count sort will need 8GB of memory.
                Console.WriteLine("> Generating array...");
                var target = Enumerable.Range(0, size)
                    .Select(_ => random.Next(0, 1_000_000))
                    .ToArray();

#if DEBUG
                var answer = target.OrderBy(i => i).ToArray();
#endif

                foreach (var algorithm in Algorithms)
                {
                    Console.WriteLine($"> Running {algorithm.Name}...");
                    var elapsed = algorithm.BenchAlgorithm(target
#if DEBUG
                        , answer
#endif
                        );
                    Console.WriteLine($"took {elapsed} ticks.");
                    Results.AddResult(size, algorithm.Name, elapsed);
                }

                Console.WriteLine();
            }

            Console.WriteLine("* Saving result to result.csv...");
            Results.SaveAsCsv("result.csv");

            Console.WriteLine("* Saving plot to plot.svg...");
            Results.SavePlotAsSvg("plot.svg");

            Console.WriteLine("* Saving plot to plot.pdf...");
            Results.SavePlotAsSvg("plot.pdf");
        }
    }
}
