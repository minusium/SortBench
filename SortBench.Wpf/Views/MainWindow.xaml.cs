using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SortBench.Core;
using SortBench.Wpf.ViewModels;

namespace SortBench.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AlgorithmsListView.SelectAll();
        }

        private void AlgorithmsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (MainWindowViewModel)DataContext;
            vm.SelectedAlgorithms = AlgorithmsListView.SelectedItems.Cast<ISortAlgorithm>().ToList();
        }

        private void MainWindow_OnClosed(object? sender, EventArgs e)
        {
            var startCommand = ((MainWindowViewModel)DataContext).StartCommand;
            if (startCommand.IsRunning)
                startCommand.Cancel();
        }
    }
}
