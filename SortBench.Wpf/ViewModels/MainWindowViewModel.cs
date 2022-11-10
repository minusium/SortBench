using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SortBench.Core;
using SortBench.Wpf.Views;

namespace SortBench.Wpf.ViewModels
{
    internal class MainWindowViewModel : ObservableObject
    {
        private uint _startSize = 10;
        private uint _endSize = 100_000;
        private uint _step = 5;
        private int _max = int.MaxValue;
        private List<ISortAlgorithm> _selectedAlgorithms = new();
        private uint _progress;

        public MainWindowViewModel()
        {
            StartCommand = new AsyncRelayCommand(DoWork);
            CancelCommand = StartCommand.CreateCancelCommand();
        }

        public uint StartSize
        {
            get => _startSize;
            set
            {
                SetProperty(ref _startSize, value);
                OnPropertyChanged(nameof(RequiredMemory));
                OnPropertyChanged(nameof(MaxProgress));
            }
        }

        public uint EndSize
        {
            get => _endSize;
            set
            {
                SetProperty(ref _endSize, value);
                OnPropertyChanged(nameof(RequiredMemory));
                OnPropertyChanged(nameof(MaxProgress));
            }
        }

        public uint Step
        {
            get => _step;
            set => SetProperty(ref _step, value);
        }

        public int Max
        {
            get => _max;
            set
            {
                SetProperty(ref _max, value);
                OnPropertyChanged(nameof(RequiredMemory));
            }
        }

        public List<ISortAlgorithm> AllAlgorithms { get; } = ISortAlgorithm.Algorithms.ToList();

        public List<ISortAlgorithm> SelectedAlgorithms
        {
            get => _selectedAlgorithms;
            set
            {
                SetProperty(ref _selectedAlgorithms, value);
                OnPropertyChanged(nameof(RequiredMemory));
            }
        }

        public ulong RequiredMemory => SelectedAlgorithms.Select(algorithm => algorithm.CalculateRequiredMemory(EndSize, Max)).OrderByDescending(size => size).FirstOrDefault(0u);

        public uint Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public uint MaxProgress => EndSize - StartSize;

        public IAsyncRelayCommand StartCommand { get; }

        public ICommand CancelCommand { get; }

        private Task DoWork(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var results = new ResultContainer();

                foreach (var algorithm in SelectedAlgorithms)
                {
                    results.AddColumn(algorithm.Name);
                }

                var random = new Random();

                for (var size = (int)StartSize; size <= EndSize; size += (int)Step)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Progress = (uint)size - StartSize;
                    });

                    var target = Enumerable.Range(0, size)
                        .Select(_ => random.Next(0, Max))
                        .ToArray();

                    foreach (var algorithm in SelectedAlgorithms)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        var elapsed = algorithm.Benchmark(target);
                        results.AddResult(size, algorithm.Name, elapsed);
                    }

                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var plotWindow = new PlotWindow
                    {
                        DataContext = new PlotViewModel
                        {
                            Model = results.GeneratePlotModel(),
                        },
                        Owner = Application.Current.MainWindow,
                    };
                    plotWindow.Show();
                });
            }, cancellationToken);
        }
    }
}
