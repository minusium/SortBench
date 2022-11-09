using System.Collections.Generic;
using System.IO;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace SortBench.Core
{
    public class ResultContainer
    {
        private readonly Dictionary<int, long[]> _data = new();
        private readonly List<string> _columns = new();

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
            // CSV Format:
            // Set separator: sep=;
            // Header: Column1;Column2;Column3;...
            // Rows: 12;10;16;...

            using var writer = new StreamWriter(fileName);

            writer.WriteLine("sep=;");

            // write header
            var columnNames = "Size;";
            columnNames += string.Join(';', _columns);
            writer.WriteLine(columnNames);

            // write each row
            foreach (var row in _data)
            {
                var columns = $"{row.Key};";
                columns += string.Join(';', row.Value);
                writer.WriteLine(columns);
            }
        }

        public PlotModel GeneratePlotModel()
        {
            // create the plot model
            var plotModel = new PlotModel();
            // Size Axis
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Size",
                MajorGridlineStyle = LineStyle.Dash,
                AxisTitleDistance = 25,
            });
            // Time Axis
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Time",
                Unit = "Ticks",
                MajorGridlineStyle = LineStyle.Dash,
                AxisTitleDistance = 25,
            });

            // Define Legends
            plotModel.Legends.Add(new Legend
            {
                LegendBorder = OxyColors.Black,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomRight,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPadding = 25,
            });

            // add a line series for each algorithm
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
    }
}
