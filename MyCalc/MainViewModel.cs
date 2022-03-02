using Calculation;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyCalc;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _calcExpression = string.Empty;
    [ObservableProperty]
    private string _answer = string.Empty;
    [ObservableProperty]
    private int _roundAccuracy = 2;

    public ObservableCollection<int> AvailableAccuracy { get; set; }
    public ObservableCollection<string> Variables { get; set; } = new();
    public ObservableCollection<string> History { get; set; } = new();

    public MainViewModel()
    {
        AvailableAccuracy = new ObservableCollection<int>(new[] { 0, 1, 2, 3, 4, 5, 6 });
    }

    private IRelayCommand? _executeCommand;
    public IRelayCommand Execute
        => _executeCommand ??= new RelayCommand(()
            =>
        {
            try
            {
                var calc = new SmartCalculator(Variables.Select(x => x.ToLower()));
                double result = calc.Execute(CalcExpression, RoundAccuracy);
                Answer = result.ToString();

                if(History.Contains(CalcExpression))
                    History.Add(CalcExpression);
            }
            catch
            {
                //empty
            }
        });
}
