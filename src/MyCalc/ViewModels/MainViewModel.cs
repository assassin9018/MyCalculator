using CalcAppShared.Services;
using CalcAppShared.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using MyCalc.Views;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace MyCalc.ViewModels;

public partial class MainViewModel : MainViewModelBase
{
    [ObservableProperty]
    private PlotModel _plot = new();

    public MainViewModel() : base(null!)
    {

    }

    public MainViewModel(IDialogService dialogService) : base(dialogService)
    {
    }

    protected override void Redraw(IEnumerable<PointF> points)
    {
        Plot.Series.Clear();
        LineSeries series = new();
        series.Points.AddRange(points.Select(x => new DataPoint(x.X, x.Y)));
        Plot.Series.Add(series);
    }

    protected override void ShutDown() => Application.Current.Shutdown();
    protected override void ShowReadMe()
    {
        HelpWindow window = new();
        window.ShowDialog();
    }
}
