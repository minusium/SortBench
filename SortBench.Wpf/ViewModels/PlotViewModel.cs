using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Wpf;
using SvgExporter = OxyPlot.SvgExporter;

namespace SortBench.Wpf.ViewModels
{
    internal class PlotViewModel : ObservableObject
    {
        private PlotModel? _model = null;

        public PlotModel? Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public PlotViewModel()
        {
            SavePdfCommand = new RelayCommand(DoSavePdf);
            SaveSvgCommand = new RelayCommand(DoSaveSvg);
        }

        public ICommand SavePdfCommand { get; }

        public ICommand SaveSvgCommand { get; }

        private void DoSavePdf()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Document (*.pdf)|*.pdf",
                Title = "Save as PDF to...",
            };
            if (saveFileDialog.ShowDialog() != true) return;

            using var file = File.Open(saveFileDialog.FileName, FileMode.Create);
            var exporter = new OxyPlot.SkiaSharp.PdfExporter
            {
                Width = 1920,
                Height = 1080,
            };
            exporter.Export(Model, file);
            file.Close();

            try
            {
                Process.Start(new ProcessStartInfo(saveFileDialog.FileName)
                {
                    UseShellExecute = true,
                });
            }
            catch
            {
                // ignore
            }
        }

        private void DoSaveSvg()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Scalable Vector Graphic (*.svg)|*.svg",
                Title = "Save as SVG to...",
            };
            if (saveFileDialog.ShowDialog() != true) return;

            using var file = File.Open(saveFileDialog.FileName, FileMode.Create);
            var exporter = new SvgExporter
            {
                Width = 1920,
                Height = 1080,
            };
            exporter.Export(Model, file);
            file.Close();

            try
            {
                Process.Start(new ProcessStartInfo(saveFileDialog.FileName)
                {
                    UseShellExecute = true,
                });
            }
            catch
            {
                // ignored
            }
        }
    }
}
