using Calculation;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MyCalc.Models;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyCalc.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _calcExpression = string.Empty;
    [ObservableProperty]
    private double _answer;
    [ObservableProperty]
    private int _roundAccuracy = 2;
    [ObservableProperty]
    private VariableModel? _selectedVariable;
    [ObservableProperty]
    private double _from;
    [ObservableProperty]
    private double _to;
    [ObservableProperty]
    private double _step;
    [ObservableProperty]
    private PlotModel _plot = new();
    [ObservableProperty]
    private bool _plotMode = false;

    public ObservableCollection<int> AvailableAccuracy { get; set; } = new(new[] { 0, 1, 2, 3, 4, 5, 6 });
    public ObservableCollection<VariableModel> Variables { get; set; } = new();
    public ObservableCollection<string> History { get; set; } = new();

    public MainViewModel()
    {
        Plot.Title = "График";
    }

    private void BuildPlot()
    {
        if(To <= From)
            throw new ArgumentException("From have to be less than To.");
        if(Step <= 0)
            throw new ArgumentException("Step should be more than 0.");

        SmartCalculator calc = new(Variables.Select(x => x.Name));

        var variablesTrees = Variables
            .Where(x => !string.IsNullOrEmpty(x.Expression))
            .ToDictionary(key => key.Name, value => calc.Parse(value.Expression, RoundAccuracy));
        List<string> realVarialbeNames = new();
        foreach(var variable in Variables.Where(x => string.IsNullOrEmpty(x.Expression)))
        {
            string name = variable.Name;
            variablesTrees.Add(name, new VariableNode(name));
            realVarialbeNames.Add(name);
        }


        IExpressionNode head = calc.Parse(CalcExpression, RoundAccuracy);
        LineSeries series = new();
        for(double i = From; i < To; i += Step)
        {
            ValueNode node = new(i);
            foreach(string name in realVarialbeNames)
                variablesTrees[name] = node;
            head.Recalculate(variablesTrees);
            series.Points.Add(new(i, head.Value));
        }

        Plot.Series.Clear();
        Plot.Series.Add(series);
    }

    private IRelayCommand? _executeCommand;
    public IRelayCommand Execute
        => _executeCommand ??= new RelayCommand(()
            =>
        {
            try
            {
                if(PlotMode)
                    BuildPlot();
                else
                    Answer = new SmartCalculator().Execute(CalcExpression, RoundAccuracy);

                if(!History.Contains(CalcExpression))
                    History.Add(CalcExpression);
            }
            catch
            {
                //empty
            }
        });

    private IRelayCommand? _addVariableCommand;
    public IRelayCommand AddVariable
        => _addVariableCommand ??= new RelayCommand(()
            =>
        {
            try
            {
                VariableModel model = new();
                Variables.Add(model);
                SelectedVariable = model;
            }
            catch
            {
                //empty
            }
        });


    private IRelayCommand? _removeVariableCommand;
    public IRelayCommand RemoveVariable
        => _removeVariableCommand ??= new RelayCommand(()
            =>
        {
            try
            {
                if(SelectedVariable is not null)
                    Variables.Remove(SelectedVariable);
            }
            catch
            {
                //empty
            }
        });
}
