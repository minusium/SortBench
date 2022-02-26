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
    }
}
