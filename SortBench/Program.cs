using System.Diagnostics;
using SortBench.Core;

namespace SortBench
{
    internal static class Program
    {
        private static readonly ResultContainer Results = new();

        private static void SavePlotAsSvg(this OxyPlot.PlotModel plotModel, string fileName)
        {
            using var file = File.Open(fileName, FileMode.Create, FileAccess.Write);
            var exporter = new OxyPlot.SvgExporter { Width = 1920, Height = 1080 };
            exporter.Export(plotModel, file);
        }

        private static void SavePlotAsPdf(this OxyPlot.PlotModel plotModel, string fileName)
        {
            using var file = File.Open(fileName, FileMode.Create, FileAccess.Write);
            var exporter = new OxyPlot.SkiaSharp.PdfExporter { Width = 1920, Height = 1080 };
            exporter.Export(plotModel, file);
        }

        public static void Main()
        {
            // add column for each algorithm to the result container
            foreach (var algorithm in ISortAlgorithm.Algorithms)
            {
                Results.AddColumn(algorithm.Name);
            }

            var random = new Random();

            // generate random array of different sizes
            for (int size = 10; size <= 10_000; size += 5)
            {
                Console.WriteLine($"* Running with size {size}");

                // set a limit for maximum number, without it maximum number is 2,147,483,647 so count sort will need 8GB of memory.
                Console.WriteLine("> Generating array...");
                var target = Enumerable.Range(0, size)
                    .Select(_ => random.Next(0, 1_000_000))
                    .ToArray();

                // sort the array with each algorithm and save the result
                foreach (var algorithm in ISortAlgorithm.Algorithms)
                {
                    Console.WriteLine($"> Running {algorithm.Name}...");
                    var elapsed = algorithm.Benchmark(target);
                    Console.WriteLine($"took {elapsed} ticks.");
                    Results.AddResult(size, algorithm.Name, elapsed);
                }

                Console.WriteLine();
            }

            // save the results to file

            Console.WriteLine("* Saving result to result.csv...");
            Results.SaveAsCsv("result.csv");

            // create plot and save to file

            var plotModel = Results.GeneratePlotModel();

            Console.WriteLine("* Saving plot to plot.svg...");
            plotModel.SavePlotAsSvg("plot.svg");

            Console.WriteLine("* Saving plot to plot.pdf...");
            plotModel.SavePlotAsPdf("plot.pdf");
        }
    }
}
