using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SortBench.Wpf.Converters
{
    internal class HumanizeSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ulong size) return "<Unsupported>";

            var result = (double)size / 1024;
            if (result < 1024)
            {
                return $"{result:#0.###} KB";
            }
                
            result /= 1024;
            if (result < 1024)
            {
                return $"{result:#0.###} MB";
            }

            result /= 1024;
            return $"{result:#0.###} GB";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
