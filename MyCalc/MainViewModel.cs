using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MyCalc;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _calcExpresion = string.Empty;
    [ObservableProperty]
    private int _roundAccuracy = 2;

    public ObservableCollection<int> AvailableAccuracy { get; set; }
    public ObservableCollection<int> History { get; set; }

    public MainViewModel()
    {
        AvailableAccuracy = new ObservableCollection<int>(new[] { 0, 1, 2, 3, 4, 5, 6 });
    }
}
