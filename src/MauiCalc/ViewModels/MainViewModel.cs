using CalcAppShared.Services;
using CalcAppShared.ViewModels;

namespace MauiCalc.ViewModels;

public partial class MainViewModel : MainViewModelBase
{
    //[ObservableProperty]
    //private PlotModel _plot = new();

    public MainViewModel() : base(null!)
    {

    }

    public MainViewModel(IDialogService dialogService) : base(dialogService)
    {
    }

    protected override void Redraw(IEnumerable<System.Drawing.PointF> points)
    {
        //Plot.Series.Clear();
        //LineSeries series = new();
        //series.Points.AddRange(points.Select(x => new DataPoint(x.X, x.Y)));
        //Plot.Series.Add(series);
    }

    protected override void ShutDown() => Application.Current?.Quit();
    protected override void ShowReadMe()
    {
        //HelpWindow window = new();
        //window.ShowDialog();
    }
}
