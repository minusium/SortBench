using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Legends;

namespace SortBench
{
    public class ResultContainer
    {
        private Dictionary<int, long[]> _data = new();
        private List<string> _columns = new();

        public void AddColumn(string name)
        {
            _columns.Add(name);
        }

        public void AddResult(int size, string name, long time)
        {
            if (!_data.ContainsKey(size))
            {
                _data[size] = new long[_columns.Count];
            }
            _data[size][_columns.IndexOf(name)] = time;
        }

        public void SaveAsCsv(string fileName)
        {
            using var writer = new StreamWriter(fileName);

            writer.WriteLine("sep=;");

            var columnNames = "Size;";
            columnNames += string.Join(';', _columns);
            writer.WriteLine(columnNames);

            foreach (var row in _data)
            {
                var columns = $"{row.Key};";
                columns += string.Join(';', row.Value);
                writer.WriteLine(columns);
            }
        }

        public PlotModel GeneratePlotModel()
        {
            var plotModel = new PlotModel();
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Size",
                MajorGridlineStyle = LineStyle.Dash,
                AxisTitleDistance = 25,
            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Time",
                Unit = "Ticks",
                MajorGridlineStyle = LineStyle.Dash,
                AxisTitleDistance = 25,
            });

            plotModel.Legends.Add(new Legend
            {
                LegendBorder = OxyColors.Black,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomRight,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPadding = 25,
            });

            for (int i = 0; i < _columns.Count; i++)
            {
                var series = new LineSeries
                {
                    Title = _columns[i],
                    ItemsSource = _data.Select(row => new DataPoint(row.Key, row.Value[i])).ToArray(),
                };
                plotModel.Series.Add(series);
            }

            return plotModel;
        }

        public void SavePlotAsSvg(string fileName)
        {
            var plotModel = GeneratePlotModel();
            using var file = File.Open(fileName, FileMode.Create, FileAccess.Write);
            var exporter = new SvgExporter { Width = 1920, Height = 1080 };
            exporter.Export(plotModel, file);
        }

        public void SavePlotAsPdf(string fileName)
        {
            var plotModel = GeneratePlotModel();
            using var file = File.Open(fileName, FileMode.Create, FileAccess.Write);
            var exporter = new OxyPlot.SkiaSharp.PdfExporter { Width = 1920, Height = 1080 };
            exporter.Export(plotModel, file);
        }
    }
}
